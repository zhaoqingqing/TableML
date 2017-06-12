## 前言

项目地址：https://github.com/zhaoqingqing/TableML

## 读懂配置文件

### 打开配置文件

打开tablemlGUI.exe所在的目录，找到**app.config**

（建议使用**notepad++，sublime text，editplus**等，可以高亮xml文件的关键词和语法，减少改错几率）

用文本或其它工具打开app.config，每一条重要的配置项都添加了注释说明(`<!--中间是注释说明 -->`)，比如：

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

