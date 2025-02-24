// 用户列表数据
let users = [];

// 初始化页面
document.addEventListener('DOMContentLoaded', () => {
    loadUsers();
});

// 加载用户列表
async function loadUsers() {
    try {
        const response = await fetch('/api/Users');
        users = await response.json();
        renderUserTable();
    } catch (error) {
        showError('加载用户列表失败');
    }
}

// 渲染用户表格
function renderUserTable() {
    const tbody = document.getElementById('userTableBody');
    tbody.innerHTML = '';

    users.forEach(user => {
        const tr = document.createElement('tr');
        tr.innerHTML = `
            <td>${user.Id}</td>
            <td>${user.Username}</td>
            <td>${user.Email}</td>
            <td>${user.Role}</td>
            <td>${user.IsActive ? '激活' : '禁用'}</td>
            <td>
                <button class="btn btn-sm btn-primary me-2" onclick="editUser(${user.Id})">编辑</button>
                <button class="btn btn-sm btn-danger" onclick="deleteUser(${user.Id})">删除</button>
            </td>
        `;
        tbody.appendChild(tr);
    });
}

// 保存用户
async function saveUser() {
    const userId = document.getElementById('userId').value;
    const user = {
        username: document.getElementById('username').value,
        password: document.getElementById('password').value,
        email: document.getElementById('email').value,
        role: document.getElementById('role').value,
        isActive: document.getElementById('isActive').checked,
        permissions: getSelectedPermissions()
    };

    try {
        let response;
        if (userId) {
            user.id = parseInt(userId);
            response = await fetch(`/api/Users/${userId}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(user)
            });
        } else {
            response = await fetch('/api/Users', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(user)
            });
        }

        if (response.ok) {
            await loadUsers();
            closeModal();
            showSuccess(userId ? '用户更新成功' : '用户创建成功');
        } else {
            throw new Error('操作失败');
        }
    } catch (error) {
        showError('保存用户失败');
    }
}

// 编辑用户
function editUser(id) {
    console.log(id);
    
    const user = users.find(u => u.Id === id);
    if (user) {
        document.getElementById('userId').value = user.Id;
        document.getElementById('username').value = user.Username;
        document.getElementById('password').value = '';
        document.getElementById('email').value = user.Email;
        document.getElementById('role').value = user.Role;
        document.getElementById('isActive').checked = user.IsActive;
        setPermissions(user.permissions);
        
        document.getElementById('modalTitle').textContent = '编辑用户';
        const modal = new bootstrap.Modal(document.getElementById('userModal'));
        modal.show();
    }
}

// 删除用户
async function deleteUser(id) {
    if (await confirmDelete()) {
        try {
            const response = await fetch(`/api/Users/${id}`, {
                method: 'DELETE'
            });
            
            if (response.ok) {
                await loadUsers();
                showSuccess('用户删除成功');
            } else {
                throw new Error('删除失败');
            }
        } catch (error) {
            showError('删除用户失败');
        }
    }
}

// 获取选中的权限
function getSelectedPermissions() {
    const permissions = [];
    document.querySelectorAll('#permissions input[type="checkbox"]:checked').forEach(checkbox => {
        permissions.push(checkbox.value);
    });
    return JSON.stringify(permissions);
}

// 设置权限选中状态
function setPermissions(permissionsJson) {
    const permissions = JSON.parse(permissionsJson || '[]');
    document.querySelectorAll('#permissions input[type="checkbox"]').forEach(checkbox => {
        checkbox.checked = permissions.includes(checkbox.value);
    });
}

// 关闭模态框
function closeModal() {
    const modal = bootstrap.Modal.getInstance(document.getElementById('userModal'));
    modal.hide();
    resetForm();
}

// 重置表单
function resetForm() {
    document.getElementById('userForm').reset();
    document.getElementById('userId').value = '';
    document.getElementById('modalTitle').textContent = '添加用户';
}

// 确认删除
function confirmDelete() {
    return Swal.fire({
        title: '确认删除',
        text: '确定要删除这个用户吗？',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: '确定',
        cancelButtonText: '取消'
    }).then(result => result.isConfirmed);
}

// 显示成功消息
function showSuccess(message) {
    Swal.fire({
        title: '成功',
        text: message,
        icon: 'success'
    });
}

// 显示错误消息
function showError(message) {
    Swal.fire({
        title: '错误',
        text: message,
        icon: 'error'
    });
}