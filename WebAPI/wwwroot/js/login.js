/**
 * 登录页面的JavaScript逻辑
 */

document.addEventListener('DOMContentLoaded', () => {
    // 检查是否已登录
    checkLoginStatus();
    
    // 处理登录表单提交
    setupLoginForm();
    
    // 加载记住的用户名
    loadRememberedUsername();
});

/**
 * 检查用户是否已登录
 */
function checkLoginStatus() {
    const token = localStorage.getItem('token');
    if (token) {
        // 已登录，跳转到管理页面
        window.location.href = '/admin/users.html';
    }
}

/**
 * 设置登录表单提交事件
 */
function setupLoginForm() {
    const loginForm = document.getElementById('loginForm');
    loginForm.addEventListener('submit', async (e) => {
        e.preventDefault();
        
        const username = document.getElementById('username').value;
        const password = document.getElementById('password').value;
        const rememberMe = document.getElementById('rememberMe').checked;
        
        try {
            const response = await fetch('/api/Users/loginAdmin', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ username, password })
            });
            
            if (response.ok) {

                console.log("11");
                
                handleLoginSuccess(response, username, rememberMe);
            } else {
                handleLoginError(response);
            }
        } catch (error) {
            showErrorMessage(error.message || '连接服务器失败');
        }
    });
}

/**
 * 处理登录成功
 */
async function handleLoginSuccess(response, username, rememberMe) {
    const data = await response.json();
    // 保存token到localStorage
    localStorage.setItem('token', data.token);
    
    // 如果选择了记住我，保存用户名
    if (rememberMe) {
        localStorage.setItem('rememberedUsername', username);
    } else {
        localStorage.removeItem('rememberedUsername');
    }
    
    // 登录成功提示
    Swal.fire({
        title: '登录成功',
        text: '欢迎回来，',
        icon: 'success',
        timer: 1500,
        showConfirmButton: false
    }).then(() => {
        // 跳转到用户管理页面
        window.location.href = '/admin/users.html';
    });
}

/**
 * 处理登录错误
 */
async function handleLoginError(response) {
    try {
        const errorData = await response.json();
        showErrorMessage(errorData.message || '登录失败');
    } catch (e) {
        showErrorMessage('登录失败');
    }
}

/**
 * 显示错误消息
 */
function showErrorMessage(message) {
    Swal.fire({
        title: '登录失败',
        text: message,
        icon: 'error'
    });
}

/**
 * 加载记住的用户名
 */
function loadRememberedUsername() {
    const rememberedUsername = localStorage.getItem('rememberedUsername');
    if (rememberedUsername) {
        document.getElementById('username').value = rememberedUsername;
        document.getElementById('rememberMe').checked = true;
    }
}