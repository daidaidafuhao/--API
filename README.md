# WebAPI 项目说明

## 项目介绍
这是一个基于.NET 8.0的WebAPI项目，提供了用户管理、员工信息管理等功能的后端服务。

## 目录结构
```
├── WebAPI/                      # 主项目目录
│   ├── Controllers/             # API控制器
│   ├── Data/                    # 数据访问层
│   ├── Filters/                 # 过滤器
│   ├── Middleware/              # 中间件
│   ├── Models/                  # 数据模型
│   ├── Repositories/            # 仓储层
│   └── wwwroot/                 # 静态资源
└── WebAPI.Tests/                # 单元测试项目
```

## 环境要求
- .NET SDK 8.0 或更高版本
- SQLite 数据库
- Visual Studio 2022 或 VS Code

## 编译和运行步骤

1. 克隆项目
```bash
git clone [你的仓库地址]
cd 后端API
```

2. 还原依赖包
```bash
dotnet restore
```

3. 应用数据库迁移
```bash
cd WebAPI
dotnet ef database update
```

4. 编译项目
```bash
dotnet build
```

5. 运行项目
```bash
dotnet run --project WebAPI
```

项目默认会在 https://localhost:7155 和 http://localhost:5155 启动。

## 主要功能
- 用户认证和授权
- 员工信息管理
- 培训记录管理
- 照片管理

## 技术栈
- ASP.NET Core 8.0
- Entity Framework Core
- SQLite 数据库
- Serilog 日志记录
- JWT 认证

## API文档
启动项目后，可以通过访问 https://localhost:7155/swagger 查看完整的API文档。

## 测试
运行单元测试：
```bash
dotnet test WebAPI.Tests
```

## 注意事项
- 确保已安装最新版本的.NET SDK
- 首次运行时会自动创建SQLite数据库文件
- 默认用户名和密码可在配置文件中修改