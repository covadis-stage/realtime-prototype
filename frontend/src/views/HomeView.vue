<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { startConnection, onTask, onTasks, getTasks, createTask, updateTask, compareTasks, type Task } from '../services/signalrService.ts'

const tasks = ref<Task[]>([])
const newTask = ref<Task>({ id: 0, name: '', description: '', selectedBy: null })

onMounted(async () => {
    startConnection()

    onTasks((newTasks) => {
        tasks.value = newTasks
    })
    onTask((newTask) => {
        const index = tasks.value.findIndex(task => task.id === newTask.id)
        if (index !== -1) tasks.value[index] = newTask
        else tasks.value.push(newTask)
    });

    tasks.value = await getTasks()
});

const createNewTask = () => {
    createTask(newTask.value)
    newTask.value = { id: 0, name: '', description: '', selectedBy: null }
}

const updateTaskIfChanged = (task: Task) => {
    const originalTask = tasks.value.find(t => t.id === task.id)
    if (!originalTask) return
    if (compareTasks(task, originalTask)) return
    updateTask(task)
}
</script>

<template>
    <table>
        <thead>
            <tr>
                <th>Id</th>
                <th>Title</th>
                <th>Description</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="task in tasks" :key="task.id">
                <td>{{ task.id }}</td>
                <td>
                    <input type="text" :value="task.name"
                        @input="updateTaskIfChanged({ ...task, name: ($event.target as HTMLInputElement).value })">
                </td>
                <td>
                    <input type="text" :value="task.description"
                        @input="updateTaskIfChanged({ ...task, description: ($event.target as HTMLInputElement).value })">
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <input type="text" v-model="newTask.name" />
                </td>
                <td>
                    <input type="text" v-model="newTask.description" />
                </td>
                <td>
                    <button @click="createNewTask">Add Task</button>
                </td>
            </tr>
        </tbody>
    </table>
</template>
