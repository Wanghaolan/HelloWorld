using System;
using System.IO;
using System.IO.Ports;
using System.Threading;
namespace sf
{
    /// <summary> CommPort类创建一个单例实例,关于SerialPort类
    /// of SerialPort (System.IO.Ports) </summary>
    /// <remarks> 准备好时，打开端口，端口代码如下。
    ///   <code>
    ///   CommPort com = CommPort.Instance;
    ///   com.StatusChanged += OnStatusChanged;
    ///   com.DataReceived += OnDataReceived;
    ///   com.Open();
    ///   </code>
    ///  请注意，代理用于处理状态和数据事件。
    /// 更改设置时，关闭并重新打开端口。代码如下：
    ///   <code>
    ///   CommPort com = CommPort.Instance;
    ///   com.Close();
    ///   com.PortName = "COM4";
    ///   com.Open();
    ///   </code>
    /// </remarks>
	public sealed class CommPort
    {
        public static string dataNum;
        SerialPort _serialPort;
		Thread _readThread;
		volatile bool _keepReading;

        //开始Singleton模式
        static readonly CommPort instance = new CommPort();

        // 明确的静态构造函数来告诉C＃编译器
        // 不要将类型标记为beforefieldinit
        static CommPort()
        {
        }

        CommPort()
        {
			_serialPort = new SerialPort();
			_readThread = null;
			_keepReading = false;
		}

		public static CommPort Instance
        {
            get
            {
                return instance;
            }
        }
        //结束 Singleton 部分

		//开始 Observer 部分
        public delegate void EventHandler(string param);
        public EventHandler StatusChanged;
        public EventHandler DataReceived;
        //结束 Observer 部分

		private void StartReading()
		{
			if (!_keepReading)
			{
				_keepReading = true;
				_readThread = new Thread(ReadPort);
                _readThread.Start();
			}
		}

		private void StopReading()
		{
			if (_keepReading)
			{
				_keepReading = false;
				_readThread.Join();	//block until exits，阻止直到存在。
				_readThread = null;
			}
        }

		/// <summary> 获取数据并传递 </summary>
		private void ReadPort()
		{
			while (_keepReading)
			{
				if (_serialPort.IsOpen)
				{
					byte[] readBuffer = new byte[_serialPort.ReadBufferSize + 1];
					try
					{
                        // 如果串行端口上有字节可用
                        // 读取返回“计数”字节，但不会阻止（等待）
                        // 对于剩余的字节。如果没有字节可用
                        // 在串行端口上，读取将阻塞至少一个字节
                        // 在端口上可用，直到ReadTimeout为止
                        // 已经过去了，此时将抛出TimeoutException。
                        int count = _serialPort.Read(readBuffer, 0, _serialPort.ReadBufferSize);
						String SerialIn = System.Text.Encoding.ASCII.GetString(readBuffer,0,count);
						DataReceived(SerialIn);

                        dataNum += SerialIn;

                       


                    }
					catch (TimeoutException) { }
				}
				else
				{
					TimeSpan waitTime = new TimeSpan(0, 0, 0, 0, 50);
					Thread.Sleep(waitTime);
				}
			}
		}

        /// <summary>使用当前设置打开串行端口。</summary>
        public void Open()
        {
			Close();

            try
            {
                _serialPort.PortName = Settings.Port.PortName;
                _serialPort.BaudRate = Settings.Port.BaudRate;
                _serialPort.Parity = Settings.Port.Parity;
                _serialPort.DataBits = Settings.Port.DataBits;
                _serialPort.StopBits = Settings.Port.StopBits;
                _serialPort.Handshake = Settings.Port.Handshake;

                // 设置读/写超时
                _serialPort.ReadTimeout = 50;
				_serialPort.WriteTimeout = 50;

				_serialPort.Open();
				StartReading();
            }
            catch (IOException)
            {
                StatusChanged(String.Format("{0} does not exist", Settings.Port.PortName));
            }
            catch (UnauthorizedAccessException)
            {
                StatusChanged(String.Format("{0} already in use", Settings.Port.PortName));
            }
            catch (Exception ex)
            {
                StatusChanged(String.Format("{0}", ex.ToString()));
            }

            // 更新状态
            if (_serialPort.IsOpen)
            {
                string p = _serialPort.Parity.ToString().Substring(0, 1);   //第一个字符
                string h = _serialPort.Handshake.ToString();
                if (_serialPort.Handshake == Handshake.None)
                    h = "打开"; // more descriptive than "None"

                StatusChanged(String.Format("{0}: {1} bps, {2}{3}{4}, {5}",
                    _serialPort.PortName, _serialPort.BaudRate,
                    _serialPort.DataBits, p, (int)_serialPort.StopBits, h));
            }
            else
            {
                StatusChanged(String.Format("{0} already in use", Settings.Port.PortName));
            }
        }


        /// <summary> 关闭端口 </summary>
        public void Close()
        {
			StopReading();
			_serialPort.Close();
            StatusChanged("连接关闭");
        }

        /// <summary> 获得端口状态 </summary>
        public bool IsOpen
        {
            get
            {
                return _serialPort.IsOpen;
            }
        }

        /// <summary> 获取可用端口的列表。已经打开端口
        /// are not returend. </summary>
        public string[] GetAvailablePorts()
        {
            return SerialPort.GetPortNames();
        }

        /// <summary>在追加行结束后，将数据发送到串行端口。 </summary>
        /// <param name="data">包含要发送的数据的字符串。 </param>
        public void Send(string data)
        {
            if (IsOpen)
            {
                string lineEnding = "";
                switch (Settings.Option.AppendToSend)
                {
                    case Settings.Option.AppendType.AppendCR:
                        lineEnding = "\r"; break;
                    case Settings.Option.AppendType.AppendLF:
                        lineEnding = "\n"; break;
                    case Settings.Option.AppendType.AppendCRLF:
                        lineEnding = "\r\n"; break;
                }

                _serialPort.Write(data + lineEnding);
            }
        }

    }
}