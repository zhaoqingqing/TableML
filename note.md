## 优秀Excel导出工具

例举几款excel导出工具

### tabtoy

跨平台的高性能便捷电子表格导出器：https://github.com/davyxu/tabtoy

开发语言：go

可借鉴的功能：

1. 可运行Lua


2. 跨平台的高性能便捷电子表格导出器


3. 充分利用CPU多核进行导出, 是已知的现有导出器中速度最快的

### 雨松

Excel辅助开发工具：https://www.zhihu.com/question/40879788

http://www.xuanyusong.com/archives/3971



### 问题讨论

https://www.zhihu.com/question/40879788

## 其它

将现有的tableml支持多sheet导出，思路整理

1. 目前的设计思路：每个excel/csv文件只读取第1个sheet，
2. 获取生成文件名，执行生成tsv或sql语句，这部分是每个excel生成一份。
3. 将原来的SimpleExcel改成一个SimpleSheet，对非excel文件进行处理