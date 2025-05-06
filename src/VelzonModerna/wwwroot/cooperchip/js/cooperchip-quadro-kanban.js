
/* cooperchip-quadro-kanban.js */

// =======================================================
// CONSTANTES E VARIÁVEIS GLOBAIS
// =======================================================
const BASE_URL = "/QuadroKanban";
let tasks = [];          // Array local das tarefas
let participants = [];   // Array local dos participantes

// Undo/Redo
const MAX_HISTORY = 20;
let doneActions = [];


// Filtro e ordenação
let searchQuery = "";
let sortOption = "none";

//const token = $('input[name="__RequestVerificationToken"]').val();
//'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()

// =======================================================
// FUNÇÕES DE API (FETCH)
// =======================================================

// ------------ Tarefas ------------ 
async function apiGetTasks() {
    const response = await fetch(`${BASE_URL}/Tasks`);
    if (!response.ok) {
        console.error("Falha ao obter tarefas:", response.statusText);
        return [];
    }
    return await response.json();
}

async function apiCreateTask(task) {
    const response = await fetch(`${BASE_URL}/Tasks`, {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            /*'RequestVerificationToken': token*/
        },
        body: JSON.stringify(task)
    });
    if (!response.ok) {
        const error = await response.json();
        console.error("Erro ao criar task:", error);
        return null;
    }
    return await response.json();
}

async function apiUpdateTask(task) {
    const response = await fetch(`${BASE_URL}/Tasks/${task.id}`, {
        method: "PUT",
        headers: {
            'Content-Type': 'application/json',
            /*'RequestVerificationToken': token*/
        },
        body: JSON.stringify(task)
    });
    if (!response.ok) {
        const error = await response.json();
        console.error("Erro ao atualizar task:", error);
        return null;
    }
    return await response.json();
}

async function apiDeleteTask(taskId) {
    const response = await fetch(`${BASE_URL}/Tasks/${taskId}`, {
        method: "DELETE"
    });
    if (!response.ok) {
        const error = await response.json();
        console.error("Erro ao excluir task:", error);
        return false;
    }
    return true;
}

// ------------ Participantes ------------ 
async function apiGetParticipants() {
    const response = await fetch(`${BASE_URL}/Participantes`);
    if (!response.ok) {
        console.error("Falha ao obter participantes:", response.statusText);
        return [];
    }
    return await response.json();
}


async function apiCreateParticipant(participant) {
    const response = await fetch(`${BASE_URL}/Participantes`, {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            /*'RequestVerificationToken': token*/
        },
        body: JSON.stringify(participant)
    });
    if (!response.ok) {
        const error = await response.json();
        console.error("Erro ao criar participante:", error);
        return null;
    }
    return await response.json();
}

async function apiDeleteParticipant(participantId) {
    const response = await fetch(`${BASE_URL}/Participantes/${participantId}`, {
        method: "DELETE"
    });
    if (!response.ok) {
        const error = await response.json();
        console.error("Erro ao remover participante:", error);
        return false;
    }
    return true;
}

// =======================================================
// FUNÇÕES DE INICIALIZAÇÃO
// =======================================================
async function initKanban() {
    // Carregar tasks do servidor
    tasks = await apiGetTasks();
    // Carregar participantes do servidor
    participants = await apiGetParticipants();

    renderParticipantsList();
    renderTasks();
}

// Chamamos a init
initKanban();

// =======================================================
// FUNÇÕES DO KANBAN
// =======================================================

// 1) Renderização
function renderTasks() {
    const columns = {
        todo: document.getElementById("todo"),
        review: document.getElementById("review"),
        progress: document.getElementById("progress"),
        done: document.getElementById("done"),
    };

    Object.values(columns).forEach(col => (col.innerHTML = ""));

    const visible = getFilteredSortedTasks();

    visible.forEach(task => {
        const card = document.createElement("div");
        card.className = `task-card priority-${task.priority}`;
        card.id = task.id;
        card.draggable = true;
        card.ondragstart = drag;

        let cardHTML = `<h6>${safeHTML(task.title)}</h6>`;
        if (task.description) {
            cardHTML += `<p>${safeHTML(task.description)}</p>`;
        }
        if (task.dueDate) {
            cardHTML += `<div class="due-date"><i class="bi bi-calendar"></i> ${safeHTML(task.dueDate)}</div>`;
        }

        // Participantes
        cardHTML += `<div class="participants mt-2">`;
        if (task.participants && task.participants.length > 0) {
            task.participants.forEach(pid => {
                const found = participants.find(p => p.id === pid);
                if (found) {
                    cardHTML += `
                      <span class="badge participant-badge bg-dark">
                        <i class="bi bi-person-fill"></i> ${safeHTML(found.name)}
                      </span>
                    `;
                }
            });
        }
        cardHTML += `</div>`;

        // Botão Editar
        cardHTML += `
          <button 
            class="btn btn-sm btn-outline-warning mt-2 w-100"
            onclick="showModal('${task.id}')"
          >
            <i class="bi bi-pencil"></i> Editar
          </button>
        `;

        card.innerHTML = cardHTML;
        columns[task.column].appendChild(card);
    });

    updateTaskCounts();

    // Empty states
    for (const [_, col] of Object.entries(columns)) {
        if (col.children.length === 0) {
            const emptyDiv = document.createElement("div");
            emptyDiv.className = "empty-state";
            emptyDiv.innerHTML = `
              <i class="bi bi-inbox"></i>
              <p><em>Nenhuma tarefa aqui.</em><br>
                 Clique em "Adicionar Tarefa" para criar!</p>
            `;
            col.appendChild(emptyDiv);
        }
    }
}

function updateTaskCounts() {
    document.querySelectorAll(".kanban-column").forEach(column => {
        const count = column.querySelector(".tasks").children.length;
        column.querySelector(".task-count").textContent = count;
    });
}

// 2) Filtro e Ordenação
function getFilteredSortedTasks() {
    let filtered = tasks.filter(t => matchesSearch(t, searchQuery));

    if (sortOption === "date") {
        filtered.sort((a, b) => {
            const dA = new Date(a.dueDate || "1970-01-01");
            const dB = new Date(b.dueDate || "1970-01-01");
            return dB - dA; // mais recentes primeiro
        });
    } else if (sortOption === "priority") {
        const priorityOrder = { alta: 1, media: 2, baixa: 3 };
        filtered.sort((a, b) => priorityOrder[a.priority] - priorityOrder[b.priority]);
    }

    return filtered;
}

function matchesSearch(task, query) {
    if (!query) return true;
    const q = query.toLowerCase();
    const titleMatch = task.title?.toLowerCase().includes(q);
    const descMatch = task.description?.toLowerCase().includes(q);

    let participantMatch = false;
    if (task.participants && task.participants.length > 0) {
        for (const pid of task.participants) {
            const found = participants.find(p => p.id === pid);
            if (found && found.name.toLowerCase().includes(q)) {
                participantMatch = true;
                break;
            }
        }
    }
    return (titleMatch || descMatch || participantMatch);
}

// 3) Drag & Drop
function allowDrop(ev) {
    ev.preventDefault();
}
function drag(ev) {
    ev.dataTransfer.setData("text", ev.target.closest(".task-card").id);
}
async function drop(ev) {
    ev.preventDefault();
    const taskId = ev.dataTransfer.getData("text");
    const taskElement = document.getElementById(taskId);
    const targetColumn = ev.target.closest(".tasks");
    if (taskElement && targetColumn) {
        targetColumn.appendChild(taskElement);
        await updateTaskPosition(taskId, targetColumn.id);
    }
}

async function updateTaskPosition(taskId, newColumn) {
    const idx = tasks.findIndex(t => t.id === taskId);
    if (idx === -1) return;
    const oldTask = structuredClone(tasks[idx]);
    const updatedTask = { ...oldTask, column: newColumn };

    // Chamar PUT no servidor
    const result = await apiUpdateTask(updatedTask);
    if (!result) {
        console.warn("Falha ao atualizar no servidor. Revertendo mudança...");
        // Reverter
        renderTasks();
        return;
    }

    // Local update
    tasks[idx] = result;
    
    renderTasks();
}

// 4) CRUD a partir do Modal
function showModal(taskId = null, column = null) {
    const modal = new bootstrap.Modal(document.getElementById("taskModal"));
    const task = tasks.find(t => t.id === taskId) || {
        id: "",
        title: "",
        description: "",
        participants: [],
        column: column || "todo",
        priority: "media",
        dueDate: ""
    };

    document.getElementById("taskId").value = taskId || "";
    document.getElementById("taskTitle").value = task.title || "";
    document.getElementById("taskDescription").value = task.description || "";
    document.getElementById("taskPriority").value = task.priority || "media";
    document.getElementById("taskDueDate").value = task.dueDate || "";
    document.getElementById("taskColumn").value = task.column || "todo";

    const deleteBtn = document.getElementById("deleteBtn");
    if (taskId) {
        deleteBtn.style.display = "block";
        deleteBtn.onclick = async () => {
            if (confirm("Tem certeza que deseja excluir esta tarefa?")) {
                await removeTaskInternal(taskId, true);
                modal.hide();
            }
        };
    } else {
        deleteBtn.style.display = "none";
    }

    loadParticipantsIntoSelect(task.participants);
    modal.show();
}

function loadParticipantsIntoSelect(selectedIds = []) {
    const select = document.getElementById("taskParticipants");
    select.innerHTML = "";
    if (participants.length === 0) {
        const option = document.createElement("option");
        option.disabled = true;
        option.textContent = "Nenhum participante cadastrado";
        select.appendChild(option);
        return;
    }
    participants.forEach(p => {
        const option = document.createElement("option");
        option.value = p.id;
        option.textContent = p.name + (p.email ? ` (${p.email})` : "");
        if (selectedIds.includes(p.id)) {
            option.selected = true;
        }
        select.appendChild(option);
    });
}

async function saveTask() {
    const taskId = document.getElementById("taskId").value;
    const column = document.getElementById("taskColumn").value;
    const selectedOptions = document.getElementById("taskParticipants").selectedOptions;
    const selectedIds = Array.from(selectedOptions).map(opt => opt.value);

    const newTask = {
        id: taskId || "00000000-0000-0000-0000-000000000000",
        title: document.getElementById("taskTitle").value.trim(),
        description: document.getElementById("taskDescription").value.trim(),
        participantIds: selectedIds,
        column: column,
        priority: document.getElementById("taskPriority").value,
        dueDate: document.getElementById("taskDueDate").value
    };

    // *** DEBUG: veja o que está sendo enviado ***
    console.log("Criando/atualizando Task com:", JSON.stringify(newTask));

    let result;
    if (!taskId) {
        // CREATE
        result = await apiCreateTask(newTask);
    } else {
        // UPDATE
        const index = tasks.findIndex(t => t.id === taskId);
        if (index !== -1) {
            const oldTask = structuredClone(tasks[index]);
            result = await apiUpdateTask(newTask);
        }
    }

    if (!result) {
        console.error("Falha ao salvar task no servidor.");
        return;
    }

    bootstrap.Modal.getInstance(document.getElementById("taskModal")).hide();
    renderTasks();
}

async function removeTaskInternal(taskId, registerActionFlag) {
    const index = tasks.findIndex(t => t.id === taskId);
    if (index === -1) return;
    const oldTask = structuredClone(tasks[index]);

    // Excluir no servidor
    const success = await apiDeleteTask(taskId);
    if (!success) return;

    tasks.splice(index, 1);
    renderTasks();
}

// =======================================================
// PARTICIPANTES
// =======================================================
function showParticipantsModal() {
    const modal = new bootstrap.Modal(document.getElementById("participantsModal"));
    document.getElementById("participantName").value = "";
    document.getElementById("participantEmail").value = "";
    renderParticipantsList();
    modal.show();
}

async function addParticipant() {
    const nameField = document.getElementById("participantName");
    const emailField = document.getElementById("participantEmail");

    const nameVal = nameField.value.trim();
    const emailVal = emailField.value.trim();
    if (!nameVal) {
        alert("Por favor, insira ao menos o nome do participante.");
        return;
    }

    const newPart = {
        id: "00000000-0000-0000-0000-000000000000",
        name: nameVal,
        email: emailVal
    };

    const result = await apiCreateParticipant(newPart);
    if (!result) {
        console.error("Falha ao criar participante.");
        return;
    }

    participants.push(result);
    nameField.value = "";
    emailField.value = "";
    renderParticipantsList();
}

async function removeParticipant(id) {
    if (!confirm("Deseja realmente remover este participante?")) return;
    const success = await apiDeleteParticipant(id);
    if (!success) return;

    // remove local
    const idx = participants.findIndex(p => p.id === id);
    if (idx !== -1) {
        participants.splice(idx, 1);
        renderParticipantsList();
    }
}

function renderParticipantsList() {
    const listContainer = document.getElementById("participantsList");
    if (!listContainer) return;

    listContainer.innerHTML = "";
    if (participants.length === 0) {
        listContainer.innerHTML = `
            <li class="list-group-item bg-dark text-white">
              Nenhum participante cadastrado.
            </li>
        `;
        return;
    }

    participants.forEach(part => {
        const li = document.createElement("li");
        li.className = "list-group-item d-flex justify-content-between bg-dark text-white";
        li.innerHTML = `
          <div>
            <strong>${safeHTML(part.name)}</strong>
            ${part.email ? ` - <small>${safeHTML(part.email)}</small>` : ""}
          </div>
          <button class="btn btn-sm btn-danger" onclick="removeParticipant('${part.id}')">
            <i class="bi bi-trash"></i>
          </button>
        `;
        listContainer.appendChild(li);
    });
}

// =======================================================
// UNDO/REDO
// =======================================================



// =======================================================
// FERRAMENTAS
// =======================================================
function safeHTML(str) {
    if (!str) return "";
    return str.replace(/[&<>"']/g, function (match) {
        switch (match) {
            case "&": return "&amp;";
            case "<": return "&lt;";
            case ">": return "&gt;";
            case '"': return "&quot;";
            case "'": return "&#39;";
        }
    });
}

// Filtro e Ordenação
function onSearchInput() {
    const input = document.getElementById("searchInput");
    searchQuery = input.value.trim();
    renderTasks();
}

function onSortChange() {
    const select = document.getElementById("sortSelect");
    sortOption = select.value;
    renderTasks();
}

// Fecha modal de tarefa ao apertar ESC
document.getElementById("taskModal").addEventListener("keydown", (e) => {
    if (e.key === "Escape") {
        bootstrap.Modal.getInstance(document.getElementById("taskModal")).hide();
    }
});

