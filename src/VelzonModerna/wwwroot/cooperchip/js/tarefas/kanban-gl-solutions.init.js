/*
Template Name: Velzon - Admin & Dashboard Template
Author: Themesbrand
Co-Author: Cooperchip CPES Ltda.
Website: https://Themesbrand.com/
Contact: contato.cooperchip@gmail.com
File: kanban-gl-solutions.init.js
*/

$(document).ready(function () {
    // Variável global para armazenar o ID da tarefa em edição
    let taskIdEmEdicao = null;

    // Inicializar o Kanban
    inicializarKanban();

    function inicializarKanban() {
        obterDadosKanban();
        configurarEventos();
    }

    // Função para buscar os dados do Kanban do servidor
    async function obterDadosKanban() {
        try {
            const response = await fetch('/Tarefas/ObterDadosKanban');
            const data = await response.json();
            console.log("Dados recebidos do servidor:", data);
            renderizarKanban(data);
        } catch (error) {
            console.error("Erro na requisição:", error);
        }
    }

    // Função para renderizar o Kanban
    function renderizarKanban(tarefas) {
        const kanbanBoardContainer = $('#kanbanBoardContainer');
        kanbanBoardContainer.html(gerarKanbanHTML(tarefas));
        taskCounter();
        inicializarDragula();
    }

    // Função para gerar o HTML do Kanban
    function gerarKanbanHTML(tarefas) {
        const estados = ['Unassigned', 'ToDo', 'Inprogress', 'InReviews', 'Completed', 'New'];
        return `
            <div class="tasks-board mb-3" id="kanbanboard">
                ${estados.map(status => renderizarListaTarefas(status, tarefas)).join('')}
            </div>
        `;
    }

    // Função para renderizar uma lista de tarefas
    function renderizarListaTarefas(status, tarefas) {
        const tarefasFiltradas = tarefas.filter(tarefa => tarefa.status === status);
        const cardsHTML = tarefasFiltradas.map(renderizarTarefa).join('');

        return `
            <div class="tasks-list">
                <div class="d-flex mb-3">
                    <div class="flex-grow-1">
                        <h6 class="fs-14 text-uppercase fw-semibold mb-0">
                            ${status} <small class="badge bg-danger align-bottom ms-1 totaltask-badge">0</small>
                        </h6>
                    </div>
                </div>
                <div data-simplebar class="tasks-wrapper px-3 mx-n3">
                    <div id="${status.toLowerCase().split(" ").join("-")}-task" class="tasks">
                        ${cardsHTML}
                    </div>
                </div>
                <div class="my-3">
                    <button class="btn btn-soft-info w-100" data-bs-toggle="modal" data-bs-target="#creatertaskModal" data-status="${status}">
                        Add More
                    </button>
                </div>
            </div>
        `;
    }

    // Função para renderizar o HTML de uma tarefa
    function renderizarTarefa(tarefa) {
        return `
            <div class="card tasks-box" data-task-id="${tarefa.id}">
                <div class="card-body">
                    <div class="d-flex mb-2">
                        <h6 class="fs-16 mb-0 flex-grow-1 text-truncate task-title">
                            <a href="/Tarefas/TaskDetails/${tarefa.id}" class="text-body d-block">
                                ${tarefa.titulo}
                            </a>
                        </h6>
                        <div class="dropdown">
                            <a href="javascript:void(0);" class="text-muted" data-bs-toggle="dropdown" aria-expanded="false"><i class="ri-more-fill"></i></a>
                            <ul class="dropdown-menu">
                                <li>
                                    <a class="dropdown-item" href="/Tarefas/TaskDetails/${tarefa.id}">
                                        <i class="ri-eye-fill align-bottom me-2 text-muted"></i>
                                        View
                                    </a>
                                </li>
                                <li>
                                    <a class="dropdown-item editar-tarefa" href="#" data-task-id="${tarefa.id}">
                                        <i class="ri-edit-2-line align-bottom me-2 text-muted"></i>
                                        Edit
                                    </a>
                                </li>
                                <li>
                                    <a class="dropdown-item" data-bs-toggle="modal" href="#deleteRecordModal" data-bs-whatever="${tarefa.id}">
                                        <i class="ri-delete-bin-5-line align-bottom me-2 text-muted"></i>
                                        Delete
                                    </a>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <p class="text-muted">${tarefa.descricao}</p>
                </div>
            </div>
        `;
    }

    function inicializarDragula() {
        const tasks_list = obterElementosTarefas();

        let drake = dragula(tasks_list)
            .on('drag', function (el) {
                el.classList.remove('ex-moved');
            })
            .on('drop', function (el, target) {
                el.classList.add('ex-moved');

                const taskId = el.getAttribute('data-task-id');
                const novoStatus = obterStatusDaLista(target.id);

                atualizarStatusTarefa(taskId, novoStatus);

                noTaskImage();
                taskCounter();
            })
            .on('over', function (el, container) {
                container.classList.add('ex-over');
            })
            .on('out', function (el, container) {
                container.classList.remove('ex-over');
            });
    }

    // Função para obter os elementos de tarefas do Kanban
    function obterElementosTarefas() {
        return [
            document.getElementById("kanbanboard"),
            document.getElementById("unassigned-task"),
            document.getElementById("todo-task"),
            document.getElementById("inprogress-task"),
            document.getElementById("inreviews-task"),
            document.getElementById("completed-task"),
            document.getElementById("new-task")
        ];
    }

    // Função para obter o status da lista com base no ID do elemento
    function obterStatusDaLista(listaId) {
        const statusMap = {
            'unassigned-task': 'Unassigned',
            'todo-task': 'ToDo',
            'inprogress-task': 'Inprogress',
            'reviews-task': 'InReviews',
            'completed-task': 'Completed',
            'new-task': 'New'
        };
        return statusMap[listaId] || '';
    }

    // Função para atualizar o status da tarefa via AJAX
    async function atualizarStatusTarefa(taskId, novoStatus) {
        try {
            const response = await fetch('/Tarefas/AtualizarStatusTarefa', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ id: taskId, status: novoStatus })
            });
            const result = await response.json();
            console.log("Status da tarefa atualizado com sucesso!", result);
        } catch (error) {
            console.error("Erro ao atualizar o status da tarefa:", error);
        }
    }

    // Função para mostrar/esconder a imagem "No Task"
    function noTaskImage() {
        document.querySelectorAll("#kanbanboard .tasks-list").forEach(item => {
            const taskBox = item.querySelectorAll(".tasks-box");
            item.querySelector('.tasks').classList.toggle("noTask", taskBox.length === 0);
        });
    }

    // Função para atualizar o contador de tarefas nas colunas
    function taskCounter() {
        document.querySelectorAll("#kanbanboard .tasks-list").forEach(element => {
            const task_counted = element.querySelectorAll(".tasks-box").length;
            element.querySelector(".totaltask-badge").innerText = task_counted;
        });
    }

    // Configurar eventos para interação do usuário
    function configurarEventos() {
        // Delegar o evento de clique para o container do Kanban
        $('#kanbanBoardContainer').on('click', '.btn-soft-info', function () {
            const statusLista = $(this).data('status');
            $('#creatertaskModal').data('status-lista', statusLista);
            console.log("Status da Lista:", statusLista);
        });

        // Vincular o evento de clique ao botão "Adicionar"
        $('#btnAdicionarTarefa').on('click', function (event) {
            event.preventDefault();
            const statusLista = $('#creatertaskModal').data('status-lista');
            console.log("Status da lista ao submeter:", statusLista);

            // Obter os dados do formulário (incluindo o status da lista)
            const novaTarefa = obterDadosFormularioTarefa(statusLista);
            criarTarefa(novaTarefa);
        });
    }

    // Função para obter os dados do formulário de tarefa
    function obterDadosFormularioTarefa(statusLista) {
        return {
            titulo: $('#sub-tasks').val(),
            descricao: $('#task-description').val(),
            dataVencimento: $('#due-date').val(),
            status: statusLista
        };
    }

    const token = $('input[name="__RequestVerificationToken"]').val();

    // Função para criar uma nova tarefa via AJAX
    async function criarTarefa(novaTarefa) {
        try {
            const response = await fetch('/Tarefas/CriarTarefa', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': token
                },
                body: JSON.stringify(novaTarefa)
            });
            const result = await response.json();

            // Fechar o modal
            $('#creatertaskModal').modal('hide');

            // Renderizar a nova tarefa no Kanban, na lista correta
            adicionarNovaTarefaAoKanban(result);

            // Atualizar os contadores de tarefas
            taskCounter();
        } catch (error) {
            console.error('Erro ao criar a tarefa:', error);
        }
    }

    // Função para adicionar uma nova tarefa ao Kanban
    function adicionarNovaTarefaAoKanban(result) {
        const novaTarefaHtml = renderizarTarefa(result.tarefa);
        $(`#${result.status.toLowerCase().split(" ").join("-")}-task .tasks`).append(novaTarefaHtml);
    }

    // Função para abrir o modal de edição
    function abrirModalEdicao(taskId) {
        taskIdEmEdicao = taskId;
        console.log('ID da tarefa em edição:', taskIdEmEdicao);
        $('#addmemberModal').modal('show');
    }

    // Expor a função abrirModalEdicao para ser usada no HTML (onclick)
    window.abrirModalEdicao = abrirModalEdicao;
});