using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Player_shuxing;//引入玩家属性类
using System.IO;//引入输入输出类，之后做本地保存功能会用到
using System.Windows;//引入Windows窗体类，之后做界面功能会用到

namespace 回合制游戏
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isRunning = true; //游戏运行状态，控制游戏循环

            Player_shuxing.Player player = new Player_shuxing.Player(); // 创建玩家对象
            Player_shuxing.diren diren = new Player_shuxing.diren(); // 创建敌人对象
            Console.WriteLine("欢迎来到回合制游戏！[此版本暂未本地保存功能]");
            LineBreak();

            bool isNewPlayer = true; // 假设这是一个新玩家
            if (isNewPlayer)
            {
                Console.WriteLine("创建角色");
                Console.WriteLine("游戏名：");
                player.PlayerName = Console.ReadLine();
            }
            //else  曾经有角色了，直接进入游戏   [加载角色本地数据]【这个还不会，先放着】
            else
            {
                Console.WriteLine("欢迎回来，" + player.PlayerName + "！");
            }
            LineBreak();

            while (isRunning)
            {
                Console.WriteLine("请选择操作(输入数字即可)：1.查看属性 2.探索 3.退出游戏");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "0": break;
                    case "1":
                        Console.WriteLine("玩家属性：");
                        Console.WriteLine("名字：" + player.PlayerName);
                        Console.WriteLine("等级：" + player.PlayerLevel);
                        Console.WriteLine("经验：" + player.Player_Exp);
                        Console.WriteLine("金币：" + player.Player_Gold);
                        Console.WriteLine("HP：" + player.Player_HP + "/" + player.Player_Max_HP);
                        Console.WriteLine("攻击力：" + player.Player_Attack);
                        break;
                    case "2":
                        Console.WriteLine("探索中······");
                        isRunning = Explore(player, diren, isRunning);
                        break;
                    case "3":
                        Console.WriteLine("退出游戏，感谢游玩！");
                        return;
                    default:
                        break;
                }
                LineBreak();
            }
            Console.WriteLine("游戏结束，感谢游玩！");
            return;
        }

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
            //敌人等级限制为「玩家等级 ±2」，最低 1 级；BOSS 怪可 + 3（探索概率 10%）
            diren.diren_Level = random.Next(player.PlayerLevel - 2, player.PlayerLevel + 2);// 随机生成敌人等级
            if(diren.diren_Level < 1)diren.diren_Level = 1;//如果敌人等级小于0，则设置为0
            //敌人基础 HP = 玩家等级 ×15 + 50
            diren.diren_HP = player.PlayerLevel * 15 + random.Next(0,50); // 设置敌人HP
            diren.diren_Max_HP = diren.diren_HP; // 设置敌人最大HP为当前HP
            //敌人攻击 = 玩家等级 ×3 + 10
            diren.diren_Attack = player.PlayerLevel * 3 + random.Next(0,10); // 随机生成敌人攻击力
            diren.diren_give_Exp = player.PlayerLevel * 5; // 击杀敌人可获得的经验值
            int Level_chayi = diren.diren_Level - player.PlayerLevel;
            if (Level_chayi > 0)
            {
                switch (Level_chayi)
                {
                    case 1:
                        diren.diren_give_Exp *= 1.2f;
                        break;
                    case 2:
                        diren.diren_give_Exp *= 1.5f;
                        break;
                    default:
                        diren.diren_give_Exp *= 2f;
                        break;
                }
            }
            else if (Level_chayi < 0)
            {
                diren.diren_give_Exp *= 0.5f;
            }
            diren.diren_give_Gold = player.PlayerLevel * 5; // 随机生成敌人给予的金币

            Console.WriteLine("敌人属性：");
            Console.WriteLine("名字：" + diren.direnName);
            Console.WriteLine("等级：" + diren.diren_Level);
            Console.WriteLine("HP：" + diren.diren_HP + "/" + diren.diren_Max_HP);
            Console.WriteLine("攻击力：" + diren.diren_Attack);

            LineBreak();

            Console.WriteLine("1. 战斗 2. 逃跑（会被追击：扣除50%血量）3.贿赂（扣除敌人等级十倍的金币数额）");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":break;
                case "2":
                    Console.WriteLine("你选择了逃跑！");
                    if(Level_chayi <= 0)
                    {
                        Console.WriteLine("你不鸟低级怪，轻易间地离开了！");
                        LineBreak();
                        return isRunning;
                    }
                    else
                    {
                        int isRun1 = random.Next(0, 1); //50%逃脱
                        if(isRun1 == 0)
                        {
                            Console.WriteLine("逃跑失败！进入战斗！");
                            LineBreak();
                            break; //进入战斗逻辑
                        }
                        else
                        {
                            Console.WriteLine("成功在高手手中逃脱，但受了重伤");
                            player.Player_HP -= player.Player_HP / 2; // 逃跑扣除50%血量
                            player.Player_HP = Math.Max(1, player.Player_HP);
                            Console.WriteLine("如今剩余血量:" + player.Player_HP);
                            LineBreak();
                            return isRunning; //成功逃脱，继续探索
                        }
                            
                    }
                case "3":
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
                player.Player_HP = player.Player_Max_HP; // 恢复玩家HP       [暂时先直接回满血，之后做血瓶之类的]
                if (player.Player_Exp >= player.PlayerLevel * 120) // 升级条件
                {
                    player.PlayerLevel++;
                    player.Player_Exp = 0; // 重置经验值
                    Console.WriteLine("恭喜你升级了！当前等级：" + player.PlayerLevel);
                    player.Player_Attack += 5; // 提升玩家攻击力
                    player.Player_Max_HP += 20; // 提升玩家最大HP       [暂时先直接提升，之后做装备之类的]
                    player.Player_HP = player.Player_Max_HP; // 升级后恢复HP
                    Console.WriteLine("你在艰苦奋斗中获得了提升！");
                    Console.WriteLine("攻击力提升至" + player.Player_Attack);
                    Console.WriteLine("生命力提升至" + player.Player_HP);
                }
                player.Player_Gold += diren.diren_give_Gold;
                Console.WriteLine("当前经验：" + player.Player_Exp + "/" + (player.PlayerLevel * 100));
            }
            else
            {
                isRunning = false;
                if (diren.diren_HP == player.Player_HP)
                    Console.WriteLine("你和敌人同归于尽了，游戏结束！");
                else if (diren.diren_HP > player.Player_HP)
                    Console.WriteLine("敌人击败了你，游戏结束！");
                //Environment.Exit(0); // 退出游戏
            }

            return isRunning;
        }

        static bool Attack_Enemy(Player_shuxing.Player player, Player_shuxing.diren diren,bool win) // 战斗逻辑
        {
            int i = 0;
            double Attack_chengqu = 0; //攻击乘区（计算伤害的）
            double unAttack_chengqu = 0;//防御乘区(计算防御的)
            bool Attack_type1 = false; //蓄力攻击判断
            int Attack_type1_time_max = 3; //蓄力攻击冷却回合数
            int Attack_type1_time = 0; //蓄力攻击（冷却回合）
            int Attack_type1_time_chayi;
            //暂时就是简单的攻击，之后可以添加技能、暴击等复杂的战斗机制，【以及防御、回血之类的技能】
            while (player.Player_HP > 0 && diren.diren_HP > 0)
            {
                Console.WriteLine("第 " + (++i) + " 回合：");

                Attack_type1_time_chayi = Attack_type1_time_max - Attack_type1_time;

                if (Attack_type1)
                {
                    ++Attack_type1_time;
                    if (Attack_type1_time_chayi<=0)
                    {
                        Attack_type1 = false;
                        Attack_type1_time = 0;
                    }
                }
                if (i == 1) 
                    Console.WriteLine($"请选择操作(输入数字即可)：1.普通攻击 2.蓄力攻击(1.5X)【还有0冷却回合】【后期消耗耐力条,避免滥用】 3.防御（本次受到伤害 -50%）");
                else
                    Console.WriteLine($"请选择操作(输入数字即可)：1.普通攻击 2.蓄力攻击(1.5X)【还有{Attack_type1_time_chayi}冷却回合】【后期消耗耐力条,避免滥用】 3.防御（本次受到伤害 -50%）");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "0":break;
                    case "1":
                        Attack_chengqu = 1; // 普通攻击
                        unAttack_chengqu = 1; // 普通防御
                        break;
                    case "2":
                        unAttack_chengqu = 1; // 蓄力攻击不影响防御
                        if (!Attack_type1)
                        {
                            Attack_type1 = true;
                            Attack_chengqu = 1.5f; // 蓄力攻击
                        }
                        else
                        {
                            Console.WriteLine($"蓄力攻击冷却中，无法使用！({Attack_type1_time_chayi}回合后可用)【切用普通攻击】");
                            Attack_chengqu = 1; // 默认普通攻击
                        }
                        break;
                    case "3":
                        Attack_chengqu = 0.0f; // 攻击乘区
                        unAttack_chengqu = 0.5f; // 防御乘区，受到伤害减半
                        break;
                    default:
                        Console.WriteLine("无效的选择，默认执行普通攻击！");
                        Attack_chengqu = 1; // 默认普通攻击
                        unAttack_chengqu = 1; // 默认普通防御
                        break;
                }

                // 玩家攻击敌人
                double actualDamageToEnemy = player.Player_Attack * Attack_chengqu;// 计算实际伤害
                diren.diren_HP -= actualDamageToEnemy;
                diren.diren_HP = Math.Max(0, diren.diren_HP);
                Console.WriteLine("你攻击了敌人！");
                Console.WriteLine("本次攻击对" + diren.direnName + "造成了" + actualDamageToEnemy + "点伤害！");
                Console.WriteLine("敌人当前血量：" + diren.diren_HP + "/" + diren.diren_Max_HP);

                if (diren.diren_HP <= 0)
                {
                    win = true;
                    break; 
                }
                        

                Console.WriteLine(); // 输出空行分隔

                // 敌人反击
                double actualDamageToPlayer = diren.diren_Attack * unAttack_chengqu; // 计算实际伤害
                player.Player_HP -= actualDamageToPlayer;
                player.Player_HP = Math.Max(0, player.Player_HP);
                Console.WriteLine("敌人反击了你！");
                Console.WriteLine("敌人对" + player.PlayerName + "造成了" + actualDamageToPlayer + "点伤害！");
                Console.WriteLine("你当前血量：" + player.Player_HP + "/" + player.Player_Max_HP);

                LineBreak();

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