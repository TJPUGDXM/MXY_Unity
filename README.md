# MXY_Unity
《丧尸围城》demo
	项目描述：3D第三人称射击+塔防游戏，玩家设置塔或者自己攻击敌人
1.	使用UGUI实现UI面板 ，面板的显示隐藏使用Resources动态加载
2.	使用animator的BlendTree实现角色和敌人的动画，AvatarMask实现不同层的动画遮罩
3.	塔的建筑升级等预制体Resources使用动态加载
4.	敌人的AI系统，使用NavMeshAgent组件实现
5.	角色属性，塔属性等数据使用Json存储
6.	调整DrawCall，批处理等实现简单的性能优化
