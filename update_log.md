## 2020-11-14

对于自定义选项的支持：

1. 是否导出lua 

2. 是否导出tsv 

3. 是否导出csharp

## 2020-11-13

增加对导出Lua的支持

增加另一种支持，只导出字段文件为Lua(用于sqlite)

## 2020-10-29

界面上增加勾选框，是否启用sqlite功能



## 2020-8-3

1. 界面大调整，去掉旧项目的部分功能
2. 默认禁用sqlite功能，相应的隐藏部分按钮
3. 路径预定义改为KSFramework(App.config)

## 2020-7-30

1. tablemlgui默认勾选为KSFramework格式
2. 升级target 为.net 4.7 在Rider中编译通过


## 2018-12-6

1. 支持编译csv文件，格式可定制

## 2018-04-2

1. 可以编译Excel全部的子页，但速度会慢些。

## 2017-06-17

1. 可以生成sql脚本文件
2. 添加浏览文件功能

## 2017-08-18

1. 如果列名是空，把它当作是注释

## 2017-06-13

1. fix 单元格式内容为公式时解析错误的bug

## TODO

- [ ] 直接把excel的数据导出为lua文件，这样就无需解析csv