// 用户列表数据
let users = [];

// 初始化页面
document.addEventListener('DOMContentLoaded', () => {
    // 检查是否已登录
    checkLoginStatus();
    loadUsers();
    loadRoles();
});

// 检查用户是否已登录
function checkLoginStatus() {
    const token = localStorage.getItem('token');
    if (!token) {
        // 未登录，跳转到登录页面
        window.location.href = '/login.html';
    }
}

// 获取授权头
function getAuthHeader() {
    const token = localStorage.getItem('token');
    return {
        'Authorization': `Bearer ${token}`,
        'Content-Type': 'application/json'
    };
}

// 处理API响应
async function handleApiResponse(response) {
    if (response.status === 401) {
        // 令牌无效或过期，重定向到登录页面
        localStorage.removeItem('token');
        Swal.fire({
            title: '会话已过期',
            text: '请重新登录',
            icon: 'warning',
            confirmButtonText: '确定'
        }).then(() => {
            window.location.href = '/login.html';
        });
        throw new Error('未授权');
    }
    return response;
}

// 加载角色列表
async function loadRoles() {
    try {
        const response = await fetch('/api/Database/roles', {
            headers: getAuthHeader()
        });
        
        const handledResponse = await handleApiResponse(response);
        
        if (handledResponse.ok) {
            const roles = await handledResponse.json();
            const roleSelect = document.getElementById('role');
            roleSelect.innerHTML = '';
            
            // 在列表顶部添加Admin选项
            const adminOption = document.createElement('option');
            adminOption.value = 'Admin';
            adminOption.textContent = 'Admin';
            roleSelect.appendChild(adminOption);
            
            roles.forEach(role => {
                // 如果角色已经是Admin，则跳过，避免重复
                if (role.toLowerCase() !== 'admin') {
                    const option = document.createElement('option');
                    option.value = role;
                    option.textContent = role;
                    roleSelect.appendChild(option);
                }
            });
            
            // 如果没有角色数据，添加默认选项
            if (roles.length === 0) {
                const defaultOptions = [
                    { value: 'user', text: '普通用户' }
                ];
                
                defaultOptions.forEach(opt => {
                    const option = document.createElement('option');
                    option.value = opt.value;
                    option.textContent = opt.text;
                    roleSelect.appendChild(option);
                });
            }
        } else {
            throw new Error('获取角色列表失败');
        }
    } catch (error) {
        console.error('加载角色列表失败:', error);
        // 加载失败时使用默认角色
        const roleSelect = document.getElementById('role');
        roleSelect.innerHTML = `
            <option value="Admin">Admin</option>
            <option value="user">普通用户</option>
        `;
    }
}

// 加载用户列表
async function loadUsers() {
    try {
        const response = await fetch('/api/Users', {
            headers: getAuthHeader()
        });
        
        const handledResponse = await handleApiResponse(response);
        
        if (handledResponse.ok) {
            users = await handledResponse.json();
            renderUserTable();
        } else {
            throw new Error('获取用户列表失败');
        }
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
        isActive: true,
        permissions: getSelectedPermissions()
    };

    try {
        let response;
        if (userId) {
            user.id = parseInt(userId);
            response = await fetch(`/api/Users/${userId}`, {
                method: 'PUT',
                headers: getAuthHeader(),
                body: JSON.stringify(user)
            });
        } else {
            response = await fetch('/api/Users', {
                method: 'POST',
                headers: getAuthHeader(),
                body: JSON.stringify(user)
            });
        }

        const handledResponse = await handleApiResponse(response);
        
        if (handledResponse.ok) {
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
        // document.getElementById('isActive').checked = user.IsActive;
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
                method: 'DELETE',
                headers: getAuthHeader()
            });
            
            const handledResponse = await handleApiResponse(response);
            
            if (handledResponse.ok) {
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
    const permissionsContainer = document.getElementById('permissions');
    // 检查权限容器是否存在
    if (permissionsContainer) {
        document.querySelectorAll('#permissions input[type="checkbox"]:checked').forEach(checkbox => {
            permissions.push(checkbox.value);
        });
    }
    return JSON.stringify(permissions);
}

// 设置权限选中状态
function setPermissions(permissionsJson) {
    const permissions = JSON.parse(permissionsJson || '[]');
    const permissionsContainer = document.getElementById('permissions');
    // 检查权限容器是否存在
    if (permissionsContainer) {
        document.querySelectorAll('#permissions input[type="checkbox"]').forEach(checkbox => {
            checkbox.checked = permissions.includes(checkbox.value);
        });
    }
    // 如果权限容器不存在，不执行任何操作
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
        text: '此操作不可逆，确定要删除吗？',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: '确定删除',
        cancelButtonText: '取消'
    }).then(result => result.isConfirmed);
}

// 显示成功消息
function showSuccess(message) {
    Swal.fire({
        title: '成功',
        text: message,
        icon: 'success',
        timer: 2000,
        showConfirmButton: false
    });
}

// 显示错误消息
function showError(message) {
    Swal.fire({
        title: '错误',
        text: message,
        icon: 'error',
        confirmButtonText: '确定'
    });
}