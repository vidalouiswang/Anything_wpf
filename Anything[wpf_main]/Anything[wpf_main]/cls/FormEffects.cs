using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Anything
{
    /*说明
        名称空间：     Anything
        类名：         FormEffects
        作用：         完成对指定窗体的各种操作
        构造函数：     Window对象
        注意：         需要实例化使用
        作者：         Vida Wang
     */
    class FormEffects
    {

        #region 成员变量

        //窗体对象
        private Window wnd = null;

        //显示、隐藏速度
        private int show_speed = 0;
        private int hide_speed = 0;

        //控制窗体透明度的定时器
        private System.Windows.Forms.Timer tm_sh = new System.Windows.Forms.Timer();
        //控制超时的定时器
        private System.Windows.Forms.Timer tm_timeout = new System.Windows.Forms.Timer();

        //超时时间
        private int time_out = 3;

        //操作进程
        private bool is_processing = false;

        //是否已初始化完成
        private bool is_initialized = false;

        //最小最大透明度
        public double min_opa = 0.1;
        public double max_opa = 1.0;

        //指示是否已隐藏
        //private bool is_hiden = false;

        //当前正在进行的操作
        private Operation current_operation = 0;

        //当前透明度
        private double current_opacity = 1;


        //手动模式标识
        private bool ManulHiding = false;

        //指示窗体是否最大化、最小化（曾经）
        private bool IsMinimized = false;
        private bool IsMaximized = false;

        //窗体进行操作标识
        private WindowState ws;

        //指示是否位关闭操作
        private bool Close = false;
        #endregion

        #region 枚举
            public enum Operation
            {
                hiding=0,
                showing=1,
            }
        #endregion

        #region 构造函数

        /// <summary>
        /// 无参构造
        /// </summary>
        public FormEffects()
        {
            //初始化内部计时器
            this.tm_sh.Tick += new EventHandler(tm_sh_Tick);
            this.tm_timeout.Tick += new EventHandler(tm_timeout_Tick);
            this.tm_timeout.Interval = 1000;
            this.tm_sh.Interval = 2;
            this.tm_sh.Stop();
            this.tm_timeout.Start();

        }


        /// <summary>
        /// 带参构造
        /// </summary>
        /// <param name="wnd">窗体对象</param>
        /// <param name="show_speed">显示速度</param>
        /// <param name="hide_speed">隐藏速度</param>
        public FormEffects(Window wnd, int show_speed, int hide_speed,bool hiding=true,
            double MinOpacity=0.03,double MaxOpacity=1)
        {
            //初始化内部定时器
            this.tm_sh.Tick += new EventHandler(tm_sh_Tick);
            this.tm_timeout.Tick += new EventHandler(tm_timeout_Tick);
            this.tm_timeout.Interval = 1000;
            this.tm_sh.Interval = 2;
            this.tm_sh.Stop();
            

            //填充内部对象、变量
            //获取Window引用
            this.Wnd = wnd;

            //设置显示速度
            if (show_speed > 0 && show_speed <= 1000)
                this.show_speed = show_speed;
            else
                this.show_speed = 1;

            //设置隐藏速度
            if (hide_speed > 1 && hide_speed <= 1000)
                this.hide_speed = hide_speed;
            else
                this.hide_speed = 1;

            //根据初始操作设置计时器间隔
            if (hiding)
                this.tm_sh.Interval = hide_speed;
            else
                this.tm_sh.Interval = show_speed;

            //设置最小透明度
            if (MinOpacity > 0 && MinOpacity <= 1)
                this.min_opa = MinOpacity;
            else
                this.min_opa = 0.1;

            //设置最大透明度
            if (MaxOpacity > 0 && MaxOpacity <= 1)
                this.max_opa = MaxOpacity;
            else
                this.max_opa = 1;
            
            //将初始化标志置为真
            this.is_initialized = true;

        }

        #endregion

        #region 成员变量外部操作接口

        /// <summary>
        /// Wnd外部操作接口
        /// </summary>
        public Window Wnd
        {
            get
            {
                return wnd;
            }

            set
            {
                wnd = value;
                this.is_initialized = true;
            }
        }

        #endregion

        #region 计时器事件响应

        private void tm_sh_Tick(object sender , EventArgs e)
        {
            //检查初始化状态
            if (is_initialized)
            {
                //确定当前操作
                if (this.current_operation==Operation.hiding)   //确定当前操作 隐藏
                {
                    //处于手动模式 且 附带操作最小化
                    if (this.ManulHiding && this.ws==WindowState.Minimized)
                    {
                        //则向下平移窗体
                        this.wnd.Top+=1;
                    }
                    //调用隐藏函数
                    innerHide();
                }
                else                                            //确定当前操作 显示
                {

                    //处于手动模式 且 以最小化
                    if (this.ManulHiding && this.IsMinimized)
                    {
                        //向上平移
                        this.wnd.Top -= 1;
                    }

                    //调用显示函数
                    innerShow();
                    //System.Threading.Thread ts = new System.Threading.Thread()



                }
            }
        }

        /// <summary>
        /// 超时定时器响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tm_timeout_Tick(object sender ,EventArgs e)
        {

            //确定已初始化
            if (is_initialized)
            {

                //检查超时时间剩余
                if (this.time_out > 0)
                    //持续减少
                    this.time_out--;
                else
                {
                    //开始隐藏
                    this.current_operation = Operation.hiding;
                    this.tm_sh.Start();
                } 
            }
        }

        #endregion

        #region 内部操作及事件响应代码

        /// <summary>
        /// 鼠标进入窗体区域时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void WindowMouse_Enter(object sender, MouseEventArgs e)
        {
            //准备显示工作
            this.current_operation = Operation.showing;
            this.tm_sh.Interval = this.show_speed;

            //重置超时时间
            this.time_out = 3;

            //开始显示
            tm_sh.Start();

            //关闭超时定时器
            this.tm_timeout.Stop();
        }

        /// <summary>
        /// 鼠标移出窗体时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void WindowMouse_Leave(object sender , MouseEventArgs e)
        {
            //准备隐藏工作
            this.tm_sh.Interval = this.hide_speed;
            this.current_operation = Operation.hiding;

            //隐藏
            this.tm_timeout.Start();
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void WindowMouse_MouseMove(object sender, MouseEventArgs e)
        {
            //重置超时时间
            this.time_out = 3;
            Point pt= e.GetPosition((IInputElement)sender);
            Point FormSize = new Point(this.wnd.ActualWidth, this.wnd.ActualHeight);
            if (Math.Abs(pt.X-FormSize.X)<10)
            {
                if (Math.Abs(pt.Y-FormSize.Y)<10)
                {
                    MessageBox.Show("this is corner.");
                }
            }
        }

        /// <summary>
        /// 鼠标按键按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void WindowMouse_Down(object sender ,MouseEventArgs e)
        {
            try
            {
                //移动窗体
                wnd.DragMove();

            }catch
            {

            }
        }

        /// <summary>
        /// 窗体状态发生改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Window_StateChanged(object sender, EventArgs e)
        {
            //窗体处于正常模式 且 类内部标识窗体曾最小化
            if (this.wnd.WindowState==WindowState.Normal && this.IsMinimized)
            {
                //准备显示
                this.current_operation = Operation.showing;
                this.tm_sh.Interval = this.show_speed;

                //手动模式 为了向上平移
                this.ManulHiding = true;

                //开始显示
                this.tm_sh.Start();
            }
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        /// <returns></returns>
        private int innerHide()
        {
            try
            {
                if (wnd.Opacity > min_opa) //透明度未达到最小
                {

                    //减小透明度
                    this.current_opacity = wnd.Opacity -= 0.03;
                }
                else  //透明度以达到最小
                {

                    //判断定时器状态
                    if (this.tm_sh.Enabled)
                    {
                        if (this.ManulHiding && this.Close)
                        {
                            Process.GetCurrentProcess().Kill();
                        }
                        //关闭定时器
                        tm_sh.Stop();
                        tm_timeout.Stop();

                        //处在手动模式且操作为最小化
                        if (this.ManulHiding && this.ws == WindowState.Minimized)
                        {
                            //重置手动操作标识
                            this.ManulHiding = false;
                            //指示窗体以最小化
                            this.IsMinimized = true;
                            //最小化
                            this.wnd.WindowState = WindowState.Minimized;
                        }

                        //处在手动模式且操作为最大化
                        else if (this.ManulHiding && this.ws == WindowState.Maximized)
                        {
                            //指示窗体以最大化
                            this.IsMaximized = true;

                            //最大化
                            this.wnd.WindowState = WindowState.Maximized;

                            //开始显示准备工作
                            this.tm_sh.Interval = this.show_speed;
                            this.current_operation = Operation.showing;

                            //开始显示
                            this.tm_sh.Start();
                        }

                        //处在手动模式且操作为还原窗体（最大化、最小化后的还原）
                        else if (this.ManulHiding && this.ws == WindowState.Normal)
                        {
                            //指示窗体处于正常状态
                            this.IsMaximized = false;
                            this.IsMinimized = false;

                            //还原
                            this.wnd.WindowState = WindowState.Normal;

                            //开始准备显示工作
                            this.tm_sh.Interval = this.show_speed;
                            this.current_operation = Operation.showing;

                            //开始显示
                            this.tm_sh.Start();
                        }
                    }
                }
                return 0;
            }
            catch
            {
                return -1;
            }
            
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        private int innerShow()
        {
            try
            {
                //若当前透明度小于最大透明度
                if (wnd.Opacity < max_opa)

                    //增加透明度
                    this.current_opacity = wnd.Opacity += 0.03;

                //以达到最大透明度
                else
                {
                    if (this.tm_sh.Enabled)
                    {

                        //关闭定时器
                        tm_sh.Stop();

                        //处于手动模式 且 操作为最小化
                        if (this.ManulHiding && this.ws == WindowState.Minimized)
                        {
                            //关闭手动模式
                            this.ManulHiding = false;

                            //关闭最小化标识
                            this.IsMinimized = false;

                            //还原标识
                            this.ws = WindowState.Normal;
                        }

                        //处于手动模式 且 操作为最大化
                        else if (this.ManulHiding && this.ws == WindowState.Maximized)
                        {

                            //关闭手动模式
                            this.ManulHiding = false;

                            //关闭最大化标识
                            this.IsMaximized = true;

                            //还原标识
                            this.ws = WindowState.Normal;
                        }

                        //处于手动模式 且 操作为还原
                        else if (this.ManulHiding && this.ws == WindowState.Normal)
                        {

                            //关闭手动模式
                            this.ManulHiding = false;

                            //关闭最大化标识
                            this.IsMaximized = false;

                            //重置操作标识
                            this.ws = WindowState.Normal;
                        }
                        
                    }
                }  
                return 0;
            }
            catch
            {
                return -1;
            }
            
        }
        #endregion

        #region 外部操作接口

        /// <summary>
        /// 隐藏
        /// </summary>
        /// <returns></returns>
        public int Hide(bool setMin=false,WindowState ws=WindowState.Minimized,bool Close=false)
        {

            //检查是否已初始化
            if (this.is_initialized)
            {

                //准备隐藏操作
                this.current_operation = Operation.hiding;

                //开启手动模式
                this.ManulHiding = true;

                //填充操作标识
                this.ws = ws;

                //填充操作标识
                this.Close = Close;

                //开始隐藏
                this.tm_sh.Start();
                return 0;
            }
            else
                return -1;

        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        public int Show()
        {

            //检查是否正处于其他（隐藏）操作
            if (this.is_processing)
            {
                //停止其他操作
                this.is_processing = false;

                //关闭操作定时器
                if (this.tm_sh.Enabled==false)
                    this.tm_sh.Stop();
            }

            //检查是否已初始化
            if (this.is_initialized)
            {
                //开始准备显示
                this.current_operation = Operation.showing;
                this.tm_sh.Interval = this.show_speed;

                //开始显示
                this.tm_sh.Start();
            }
            else
                return -1;

            return 0;
        }

        /// <summary>
        /// 重置超时时间
        /// </summary>
        /// <param name="timeout"></param>
        public void ReSetTimeout(int timeout=3)
        {
            //重置超时
            this.time_out = timeout;
        }

        /// <summary>
        /// 设置显示速度
        /// </summary>
        /// <param name="speed"></param>
        public void SetShowSpeed(int speed)
        {
            //手动更改显示速度
            this.show_speed = speed;
        }
        /// <summary>
        /// 获取当前显示速度
        /// </summary>
        /// <returns></returns>
        public int GetShowSpeed()
        {
            //获取当前已设定的显示速度
            return this.show_speed;
        }

        /// <summary>
        /// 设置隐藏速度
        /// </summary>
        /// <param name="speed"></param>
        public void SetHideSpeed(int speed)
        {
            //手动设置隐藏速度
            this.hide_speed = speed;
        }

        /// <summary>
        /// 获取当前隐藏速度
        /// </summary>
        /// <returns></returns>
        public int GetHideSpeed()
        {
            //获取当前已设定的隐藏速度
            return this.hide_speed;
        }

        /// <summary>
        /// 设置最小透明度
        /// </summary>
        /// <param name="Opacity"></param>
        public void SetMinOpacity(double Opacity)
        {
            //手动设置最小透明度
            this.min_opa = Opacity;
        }

        /// <summary>
        /// 获取最小透明度
        /// </summary>
        /// <returns></returns>
        public double GetMinOpacity()
        {
            //获取已设定的最小透明度
            return this.min_opa;
        }

        /// <summary>
        /// 设置最大透明度
        /// </summary>
        /// <param name="Opacity"></param>
        public void SetMaxOpacity(double Opacity)
        {
            //手动设置最大透明度
            this.max_opa = Opacity;
        }

        /// <summary>
        /// 获取最大透明度
        /// </summary>
        /// <returns></returns>
        public double GetMaxOpacity()
        {
            //获取当前已设定的最大透明度
            return this.max_opa;
        }

        #endregion

    }
}
