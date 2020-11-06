### 前言

把配置表转换成lua文件，可以在lua中直接读取，而无需再解析CSV文件

### EmmlyLua格式

emmlylua 注解格式如下

```lua
---@class Billboard
---@field public id number
---@field public name string
```



把一个excel生成放到lua中的完整文件如下：

```lua
---@class Billboard
---@field public id number
---@field public name string
return {
	[1] = {
		id = 1,
		name = "Name1"
    }
	[2] = {
		id = 2,
		name = "Name2"
    }
}    
```

