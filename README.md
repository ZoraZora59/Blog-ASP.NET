# 个人博客站实例


### 介绍文档分为如下4个模块
- 项目总括
- 项目分析
- 功能解析
- 开发日志
  
## 项目总括

    本项目通过ASP.NET MVC5进行开发，依托框架和程序包主要包括:
    - .net Framework 4.5
    - bootstrap v4.3.1
    - EntityFramework v6.2.0
    - jQuery v3.3.1
    - Microsoft Asp.Net Razor v3.2.7
    - KindEditor v4.1.11
    - EasyUI v1.7.0
    开发时使用Chrome浏览器进行测试。

博客站功能分为面向所有用户的前台展示和面向管理员的后台管理两部分。
- 前台展示
  - 母版页（上部）- 博客站标题/欢迎语
  - 母版页（侧边）- 公告栏/搜索栏/分类栏/最新评论/文章推荐
  - 首页分部视图 - 文章摘要总览
  - 对应文章分部视图 - 文章详情/评论详情/评论增删
  - 用户注册/用户登录
![前台](https://raw.githubusercontent.com/ZoraZora59/Blog-ASP.NET/master/%E6%99%AE%E9%80%9A%E7%94%A8%E6%88%B7.jpg)
- 后台
  - 管理主页 - 数据统计（用户数/文章数/评论数）
  - 文章管理 - 文章 增/删/改
  - 评论管理 - 评论 删
  - 用户管理 - 用户 删
  - 分类管理 - 分类 增/删/改
  - 配置管理 - 博客站标题、欢迎语、公告栏 改
![后台](https://raw.githubusercontent.com/ZoraZora59/Blog-ASP.NET/master/%E7%AE%A1%E7%90%86%E5%91%98.jpg)
