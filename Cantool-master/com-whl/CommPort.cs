using System;
using System.IO;
using System.IO.Ports;
using System.Threading;
namespace sf
{
    /// <summary> CommPort�ഴ��һ������ʵ��,����SerialPort��
    /// of SerialPort (System.IO.Ports) </summary>
    /// <remarks> ׼����ʱ���򿪶˿ڣ��˿ڴ������¡�
    ///   <code>
    ///   CommPort com = CommPort.Instance;
    ///   com.StatusChanged += OnStatusChanged;
    ///   com.DataReceived += OnDataReceived;
    ///   com.Open();
    ///   </code>
    ///  ��ע�⣬�������ڴ���״̬�������¼���
    /// ��������ʱ���رղ����´򿪶˿ڡ��������£�
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

        //��ʼSingletonģʽ
        static readonly CommPort instance = new CommPort();

        // ��ȷ�ľ�̬���캯��������C��������
        // ��Ҫ�����ͱ��Ϊbeforefieldinit
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
        //���� Singleton ����

		//��ʼ Observer ����
        public delegate void EventHandler(string param);
        public EventHandler StatusChanged;
        public EventHandler DataReceived;
        //���� Observer ����

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
				_readThread.Join();	//block until exits����ֱֹ�����ڡ�
				_readThread = null;
			}
        }

		/// <summary> ��ȡ���ݲ����� </summary>
		private void ReadPort()
		{
			while (_keepReading)
			{
				if (_serialPort.IsOpen)
				{
					byte[] readBuffer = new byte[_serialPort.ReadBufferSize + 1];
					try
					{
                        // ������ж˿������ֽڿ���
                        // ��ȡ���ء��������ֽڣ���������ֹ���ȴ���
                        // ����ʣ����ֽڡ����û���ֽڿ���
                        // �ڴ��ж˿��ϣ���ȡ����������һ���ֽ�
                        // �ڶ˿��Ͽ��ã�ֱ��ReadTimeoutΪֹ
                        // �Ѿ���ȥ�ˣ���ʱ���׳�TimeoutException��
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

        /// <summary>ʹ�õ�ǰ���ô򿪴��ж˿ڡ�</summary>
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

                // ���ö�/д��ʱ
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

            // ����״̬
            if (_serialPort.IsOpen)
            {
                string p = _serialPort.Parity.ToString().Substring(0, 1);   //��һ���ַ�
                string h = _serialPort.Handshake.ToString();
                if (_serialPort.Handshake == Handshake.None)
                    h = "��"; // more descriptive than "None"

                StatusChanged(String.Format("{0}: {1} bps, {2}{3}{4}, {5}",
                    _serialPort.PortName, _serialPort.BaudRate,
                    _serialPort.DataBits, p, (int)_serialPort.StopBits, h));
            }
            else
            {
                StatusChanged(String.Format("{0} already in use", Settings.Port.PortName));
            }
        }


        /// <summary> �رն˿� </summary>
        public void Close()
        {
			StopReading();
			_serialPort.Close();
            StatusChanged("���ӹر�");
        }

        /// <summary> ��ö˿�״̬ </summary>
        public bool IsOpen
        {
            get
            {
                return _serialPort.IsOpen;
            }
        }

        /// <summary> ��ȡ���ö˿ڵ��б��Ѿ��򿪶˿�
        /// are not returend. </summary>
        public string[] GetAvailablePorts()
        {
            return SerialPort.GetPortNames();
        }

        /// <summary>��׷���н����󣬽����ݷ��͵����ж˿ڡ� </summary>
        /// <param name="data">����Ҫ���͵����ݵ��ַ����� </param>
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