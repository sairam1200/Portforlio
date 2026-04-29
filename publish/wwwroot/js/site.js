// Chat functionality
function toggleChat() {
    const chatBox = document.getElementById("chatBox");
    chatBox.classList.toggle("active");
}

function handleKeyPress(event) {
    if (event.key === "Enter") {
        sendMessage();
    }
}

async function sendMessage() {
    const input = document.getElementById("userInput");
    const text = input.value.trim();
    if (!text) return;

    const messages = document.getElementById("messages");

    // Add user message
    messages.innerHTML += `<div class="message user">${text}</div>`;
    input.value = "";

    // Add typing indicator
    const typingDiv = document.createElement("div");
    typingDiv.className = "message bot";
    typingDiv.innerText = "Typing...";
    messages.appendChild(typingDiv);

    messages.scrollTop = messages.scrollHeight;

    try {
        const response = await fetch('/api/Chat/GetResponse', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ message: text })
        });

        const data = await response.json();

        // Replace typing with actual reply
        typingDiv.innerText = data.reply;

    } catch (error) {
        typingDiv.innerText = "⚠️ Error connecting to AI";
        console.error(error);
    }

    messages.scrollTop = messages.scrollHeight;
}

// Close chat when clicking outside (optional)
document.addEventListener('click', function(event) {
    const chatContainer = document.getElementById("chatBox");
    const chatToggle = document.querySelector(".chat-toggle");
    
    if (!chatContainer.contains(event.target) && !chatToggle.contains(event.target)) {
        if (chatContainer.classList.contains("active")) {
            // Optionally close on outside click
        }
    }
});

// Scroll to top functionality
window.addEventListener('scroll', function() {
    const scrollBtn = document.getElementById('scrollToTop');
    if (window.pageYOffset > 300) {
        scrollBtn.style.display = 'block';
    } else {
        scrollBtn.style.display = 'none';
    }
});

document.addEventListener('DOMContentLoaded', function() {
    const scrollBtn = document.getElementById('scrollToTop');
    if (scrollBtn) {
        scrollBtn.addEventListener('click', function() {
            window.scrollTo({ top: 0, behavior: 'smooth' });
        });
    }
});