CanTool项目需求：
1.能够搜索到本机所有可使用的COM口，并在弹出式ComboBox中以列表方式让用户选择CanTool装置在上位机中映射的COM口。
2.能够实现CANtool装置的CAN速率设置、进入CAN工作状态（Open）、进入CAN初始化状态（Close）。这些设定内容可保存到CanToolApp设定文件中，供下次使用。
3.能够对接收到的多个CAN信息，通过CAN信息及CAN信号数据库进行解析，将CAN信息原始数据进行显示。并能对CAN信息中的CAN信号的物理值实时数据进行显示。
4.显示时可以让用户选择仪表盘方式显示接收到CAN信号物理值。这些用户选择的显示方式可保存到CanToolApp设定文件中，供下次使用。
5.可以让用户选择某些接收到的CAN信号，显示其变化的实时物理值曲线。
6.可以将接收到的所有CAN信息数据，实时保存为数据文件。格式为CSV格式，或自定义。
7.能够指定要发送的多个CAN信息，并允许用户设定CAN信息中的CAN信号物理值。可以指定CAN信息的发送周期（0-65535ms即0x0000-0xFFFF）。
8.App可将用户设定的物理值转换为CAN信号值，将CAN信息中包含的所有CAN信号合成完整的CAN信息后，发送给CanTool装置，发送到CAN总线上。
9.可以加载用户提供的CAN信息和信号数据库，完成CAN信号数据的解析以及CAN发送信息的组装。可以显示CAN信号在CAN信息的布局。
10.加载用户提供的CAN信息和信号数据库，可以树状结构显示在GUI界面中。
11.可以将用户提供的CAN信息和信号数据库另存为xml和JSON (JavaScript Object Notation)格式。也可以已将xml或Json格式的数据库，转换为CAN信息和信号数据库格式。
12.可以将所有CAN信息实时数据、CAN设定信息等 通过WEB API方式更新到远程数据库。此时CanToolApp作为客户端与远程的Web API服务进行数据交换。
13.功能可能随时增加或修改，需要做好变更管理。