<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { connection } from '../services/signalrService.ts'

const messages = ref<string[]>([]);
const newMessage = ref("");
const username = ref("User" + Math.floor(Math.random() * 1000));

onMounted(() => {
    connection.on("ReceiveMessage", (user: string, message: string) => {
        console.log(user, message);
        messages.value.push(`${user}: ${message}`);
    });
});

const sendMessage = async () => {
    if(newMessage.value.trim()) {
        await connection.invoke("SendMessage", username.value, newMessage.value);
        newMessage.value = "";
    }
}
</script>

<template>
    <main>
        <h2>Chat</h2>
        <ul>
            <li v-for="(msg, index) in messages" :key="index">
                {{ msg }}
            </li>
        </ul>
        <input v-model="newMessage" @keyup.enter="sendMessage" placeholder="Type a message..." />
        <button @click="sendMessage">Send</button>
    </main>
</template>
