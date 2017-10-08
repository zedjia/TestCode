using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.ClientEngine;
using Z.Lib.Model;

namespace SSWinClient
{
    public class CommonSocketClient : EasyClient
    {

        public async Task SendFile(string fileFullPath)
        {
            FileInfo fileInfo = new FileInfo(fileFullPath);
            await SendFile(fileInfo);
        }


        public async Task SendFile(FileInfo fileInfo)
        {
            //创建一个文件对象
            if (fileInfo.Name.Length > 50)
            {
                throw new Exception("文件名不能超过50.");
            }

            await Task.Run(() =>
            {
                using (FileStream fStream = fileInfo.OpenRead())
                {
                    int PacketSize = 20480 - 200;  //包的大小
                    string Id = Guid.NewGuid().ToString();
                    int PackageCount = (int)(fStream.Length / ((long)PacketSize));//包的数量
                    int LastDataPacket = (int)(fStream.Length - ((long)(PacketSize * PackageCount)));//最后一个包的大小

                    FileRequestInfo fi = new FileRequestInfo()
                    {
                        FileName = fileInfo.Name,
                        FileSize = fileInfo.Length,
                        Id = Id,
                        PackageCount = LastDataPacket == 0 ? PackageCount : PackageCount + 1
                    };
                    //数据包
                    //开始循环发送数据包
                    byte[] data = new byte[PacketSize];
                    for (int i = 0; i < PackageCount; i++)
                    {
                        //从文件流读取数据并填充数据包
                        fStream.Read(data, 0, data.Length);
                        fi.PackData = data;
                        fi.Seq = i;
                        this.Send(fi.ToSendData(CommonCommands.IMFile, i));
                    }
                    //如果还有多余的数据包，则应该发送完毕！
                    if (LastDataPacket != 0)
                    {
                        data = new byte[LastDataPacket];
                        fStream.Read(data, 0, data.Length);
                        fi.PackData = data;
                        fi.Seq = fi.PackageCount == 1 ? 0 : fi.PackageCount - 1;
                        this.Send(fi.ToSendData(CommonCommands.IMFile, PackageCount));
                    }
                    //关闭套接字
                    //client.Close();
                    //关闭文件流
                    fStream.Close();
                }

            });
            //打开文件流

        }

    }
}
