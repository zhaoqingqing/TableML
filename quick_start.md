## 前言

项目地址：https://github.com/zhaoqingqing/TableML

项目介绍：

更新日志：

### 技术支持

如果在使用过程中遇到问题或发现bug，欢迎与我联系。

我的邮箱：569032731@qq.com

## 读懂配置文件

### 打开配置文件

打开tablemlGUI.exe所在的目录，找到**app.config**

（*建议使用**notepad++，sublime text，editplus**等可以高亮xml关键词和语法，减少改错几率*）

打开app.config，每一条重要的配置项都添加了注释说明(`<!--中间是注释说明 -->`)，比如：

```
<!--是否使用绝对路径;true:所有路径都是绝对的,完整路径;false:所有路径是相对于此exe的-->
<add key="UseAbsolutePath" value="false" />

<!--excel源文件路径-->
<add key="srcExcelPath" value=".\..\Src\" />

<!--excel编译后的database保存路径-->
<add key="DBPath" value=".\..\client_setting\data.db" />
```

### 是否使用绝对路径？

**UseAbsolutePath**是一项重要配置，它决定整个应用程序中上涉及到的路径是相对路径还是绝对路径。

绝对路径是指一个文件的完整路径，完整到盘符，路径不包含..\之类的。

我是绝对路径：`c:\work\plan\005ConfigTable\client_tool\TableMLGUI.exe`

我是相对路径：`..\client_tool\TableMLGUI.exe`

### 有些路径我不知道填？

客户端读表代码路径 和 客户端项目tml路径 ，这两项如果你不需要用到，那就保持默认值，不用修改

如果你是一名策划，只需要导表，那你基本就只需要修改DBPath为客户端的路径。

## 使用篇

### 主界面预览

![https://github.com/zhaoqingqing/TableML/blob/custom/Document/tablemlgui_main.png](https://github.com/zhaoqingqing/TableML/blob/custom/Document/tablemlgui_main.png)

### 命令行模式

命令行模式目前支持批量编译全部的excel，并将数据插入到sqlite中，它是一个bat，双击就OK了。



### 编译全部或部分

- 编译并插入到sqlite中

  - 如果你需要编译全部的excel，就选择它。

    ​

- 编译指定的几个excel
  - 如果仅仅想编译指定的几个excel，把要的excel拖到框中，选择 **编译上面框中的excel**



### 辅助功能

这里是一些辅助的功能，你可以看看。

- 组- For CSharp版本使用 

  - 如果是把数据插入到sqlite中，这部分功能不会用到，我们目前就是。

  ​

- 组- 检查Excel错误

  - 当你的表导出出现错误时，可以选择性的使用这里的一些功能



- 组- 其它

  - 这里是一些文件夹打开类的功能

    ​ 

在编译excel后，会在应用程序目录生成一个**compile_result.csv**，表中记录着

`编译后表名[文件名] <=> 源始Excel文件名`方便查看（当源始Excel文件名是中文时不能做表名时）

