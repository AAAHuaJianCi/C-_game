using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Player_shuxing;//引入玩家属性类

namespace 回合制游戏
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isRunning = true;//游戏运行状态，控制游戏循环
            Player_shuxing.Player player = new Player_shuxing.Player(); // 创建玩家对象
            Player_shuxing.diren diren = new Player_shuxing.diren(); // 创建敌人对象
            Console.WriteLine("欢迎来到回合制游戏！[此版本暂未本地保存功能]");
            LineBreak();

            bool isNewPlayer = true; // 假设这是一个新玩家
            if (isNewPlayer) {
                Console.WriteLine("创建角色");
                Console.WriteLine("游戏名：");
                player.PlayerName = Console.ReadLine();
            }
            //else  曾经有角色了，直接进入游戏   [加载角色本地数据]【这个还不会，先放着】
             else {
                Console.WriteLine("欢迎回来，" + player.PlayerName + "！");
            }
            LineBreak();

            while (isRunning)
            {
                Console.WriteLine("请选择操作(输入数字即可)：1.查看属性 2.探索 3.退出游戏");
                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 0:break;
                    case 1:
                        Console.WriteLine("玩家属性：");
                        Console.WriteLine("名字：" + player.PlayerName);
                        Console.WriteLine("等级：" + player.PlayerLevel);
                        Console.WriteLine("经验：" + player.Player_Exp);
                        Console.WriteLine("金币：" + player.Player_Gold);
                        Console.WriteLine("HP：" + player.Player_HP + "/" + player.Player_Max_HP);
                        Console.WriteLine("攻击力：" + player.Player_Attack);
                        break;
                    case 2:
                        Console.WriteLine("探索中······");
                        Explore(player, diren,isRunning);
                        break;
                    case 3:
                        Console.WriteLine("退出游戏，感谢游玩！");
                        return;
                }
                LineBreak();
            }
            Console.WriteLine("游戏结束，感谢游玩！");

            static bool Explore(Player_shuxing.Player player, Player_shuxing.diren diren,bool isRunning) // 探索逻辑
            {
                bool encounterEnemy = new Random().Next(0, 2) == 0; // 50%概率遇到敌人
                if(!encounterEnemy)
                {
                    Console.WriteLine("你没有遇到任何敌人，继续探索吧！");
                    return isRunning;
                }
                bool iswin = true; // 假设玩家赢了，之后可以添加战斗逻辑来决定胜负
                Console.WriteLine("你遇到了一个敌人！");
                //随机生成敌人属性
                diren.direnName = "敌人";
                Random random = new Random(); // 创建一个随机数生成器
                diren.diren_Level = random.Next(player.PlayerLevel - 3, player.PlayerLevel + 10);// 随机生成敌人等级
                if(diren.diren_Level < 1)diren.diren_Level = 1;//如果敌人等级小于0，则设置为0
                diren.diren_HP = random.Next(player.PlayerLevel*5, player.PlayerLevel * 10) + player.PlayerLevel * 20; // 随机生成敌人HP
                diren.diren_Max_HP = diren.diren_HP; // 设置敌人最大HP为当前HP
                diren.diren_Attack = random.Next(player.PlayerLevel * 2, player.PlayerLevel * 5) + player.PlayerLevel * 10; // 随机生成敌人攻击力
                diren.diren_give_Exp = player.PlayerLevel * 10; // 随机生成敌人给予的经验值
                diren.diren_give_Gold = player.PlayerLevel * 5; // 随机生成敌人给予的金币

                Console.WriteLine("敌人属性：");
                Console.WriteLine("名字：" + diren.direnName);
                Console.WriteLine("等级：" + diren.diren_Level);
                Console.WriteLine("HP：" + diren.diren_HP + "/" + diren.diren_Max_HP);
                Console.WriteLine("攻击力：" + diren.diren_Attack);

                LineBreak();

                Console.WriteLine("1. 战斗 2. 逃跑（会被追击：扣除50%血量）3.贿赂（扣除敌人等级十倍的金币数额）");
                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:break;
                    case 2:
                        Console.WriteLine("你选择了逃跑！");
                        player.Player_HP -= player.Player_HP / 2; // 逃跑扣除50%血量
                        Console.WriteLine("你在逃跑过程中受到了伤害，扣除了50%血量！");
                        LineBreak();
                        return isRunning;
                    case 3:
                        int bribeAmount = diren.diren_Level * 10; // 贿赂金额为敌人等级的十倍
                        if (player.Player_Gold >= bribeAmount)
                        {
                            player.Player_Gold -= bribeAmount; // 扣除金币
                            Console.WriteLine("你成功贿赂了敌人，扣除了 " + bribeAmount + " 金币！");
                            LineBreak();
                            return isRunning;
                        }
                        else
                        {
                            Console.WriteLine("你的金币不足以贿赂敌人，继续战斗吧！");
                            break;
                        }
                }
                LineBreak();

                //战斗逻辑
                iswin = Attack_Enemy(player, diren,iswin);

                // 结束后，更新玩家属性
                if (iswin)
                {
                    Console.WriteLine("你击败了敌人！");
                    Console.WriteLine("你获得了 " + diren.diren_give_Exp + " 经验和 " + diren.diren_give_Gold + " 金币！");
                    player.Player_Exp += diren.diren_give_Exp;
                    if (player.Player_Exp >= player.PlayerLevel * 100) // 升级条件
                    {
                        player.PlayerLevel++;
                        player.Player_Exp = 0; // 重置经验值
                        Console.WriteLine("恭喜你升级了！当前等级：" + player.PlayerLevel);
                    }
                    player.Player_Gold += diren.diren_give_Gold;
                    Console.WriteLine("当前经验：" + player.Player_Exp + "/" + (player.PlayerLevel * 100));
                    player.Player_HP = player.Player_Max_HP; // 恢复玩家HP       [暂时先直接回满血，之后做血瓶之类的]
                    player.Player_Attack += diren.diren_Level / 2; // 提升玩家攻击力       [暂时先直接提升，之后做装备之类的]
                }
                else
                {
                    isRunning = false;
                    if (diren.diren_HP == player.Player_HP)
                        Console.WriteLine("你和敌人同归于尽了，游戏结束！");
                    else if(diren.diren_HP > player.Player_HP)
                        Console.WriteLine("敌人击败了你，游戏结束！");
                    //Environment.Exit(0); // 退出游戏
                }

                return isRunning;
            }

            static bool Attack_Enemy(Player_shuxing.Player player, Player_shuxing.diren diren,bool win) // 战斗逻辑
            {
                //暂时就是简单的攻击，之后可以添加技能、暴击等复杂的战斗机制，【以及防御、回血之类的技能】
                while (player.Player_HP > 0 && diren.diren_HP > 0)
                {
                    // 玩家攻击敌人
                    diren.diren_HP -= player.Player_Attack;
                    diren.diren_HP = Math.Max(0, diren.diren_HP);
                    Console.WriteLine("你攻击了敌人！");
                    Console.WriteLine("本次攻击对" + diren.direnName + "造成了" + player.Player_Attack + "点伤害！");
                    Console.WriteLine("敌人当前血量：" + diren.diren_HP + "/" + diren.diren_Max_HP);

                    if (diren.diren_HP <= 0)
                    {
                        win = true;
                        break; 
                    }
                        

                    Console.WriteLine(); // 输出空行分隔

                    // 敌人反击
                    player.Player_HP -= diren.diren_Attack;
                    player.Player_HP = Math.Max(0, player.Player_HP);
                    Console.WriteLine("敌人反击了你！");
                    Console.WriteLine("敌人对" + player.PlayerName + "造成了" + diren.diren_Attack + "点伤害！");
                    Console.WriteLine("你当前血量：" + player.Player_HP + "/" + player.Player_Max_HP);

                    if (player.Player_HP <= 0)
                    {
                        win = false;
                        break;
                    }
                        Console.WriteLine(); // 输出空行分隔
                }
                LineBreak();

                return win;
            }

            static void LineBreak()
            {
                Console.WriteLine("========================================");
            }

        }
    }
}