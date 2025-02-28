import * as signalR from "@microsoft/signalr";

export interface Task {
    id: number;
    name: string;
    description: string;
    selectedBy: string | null | undefined;
}

export const compareTasks = (task1: Task, task2: Task): boolean => {
    return task1.id === task2.id &&
           task1.name === task2.name &&
           task1.description === task2.description &&
           task1.selectedBy === task2.selectedBy;
};

const restUrl = "https://localhost:7229";

const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7229/taskhub")
    .withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Information)
    .build();

export const startConnection = async () => {
    try {
        await connection.start();
        console.log("Connected to hub");
    } catch (error) {
        console.error("Error connecting to SIGNALR, retrying soon", error);
        setTimeout(startConnection, 5000);
    }
};

export const onTask = (callback: ({ }: Task) => void) => {
    connection.on("ReceiveTask", callback);
};

export const onTasks = (callback: ({ }: Task[]) => void) => {
    connection.on("ReceiveTasks", callback);
}

export const getTasks = async () => {
    try {
        const response = await fetch(`${restUrl}/tasks`);
        const tasks = await response.json();
        let taskArray: Task[] = [];

        tasks.forEach((task: any) => {
            taskArray.push({
                id: task.id,
                name: task.name,
                description: task.description,
                selectedBy: task.selectedBy
            } as Task);
        });

        return taskArray;
    }
    catch (error) {
        console.error("Error getting tasks", error);
        return [];
    }
};

export const createTask = async (task: Task) => {
    try {
        const response = await fetch(`${restUrl}/tasks`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(task)
        });

        if (response.ok) {
            console.log("Task created successfully");
        } else {
            console.error("Error adding task");
        }
    } catch (error) {
        console.error("Error adding task", error);
    }
}

export const updateTask = async (task: Task) => {
    try {
        const response = await fetch(`${restUrl}/tasks/${task.id}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(task)
        });

        if (response.ok) {
            console.log("Task updated successfully");
        } else {
            console.error("Error updating task");
        }
    }
    catch (error) {
        console.error("Error updating task", error);
    }
}