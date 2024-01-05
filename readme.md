# 待办事项列表网站
* 实现用户登录功能
* 可以添加和查看待办事项

# 项目怎么跑？
* 在`./end`目录下，运行`dotnet run`命令，启动后端服务。如果提示缺少运行环境就按照提示安装即可
* 打开`./front/requests.js`文件，修改`base_url`为你的后端服务地址。这个地址可以在后端服务启动后看到。比如:
```powershell
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5144
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```
这里的地址就是`http://localhost:5144`，将`base_url`修改为这个地址即可。
* 打开`./front/index.html`文件，用浏览器打开即可
