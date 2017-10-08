using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Command;
using Z.Lib.Model;

//using WC.Lib.Model;

namespace SSWinServer.Handler
{
    //public abstract class CommonBaseCommand<T> : CommandBase<CommonBinarySession, CommonRequestInfo> where T : struct 
    //{
    //    public override void ExecuteCommand(CommonBinarySession session, CommonRequestInfo requestInfo)
    //    {
    //        //using (var stream = new MemoryStream(requestInfo.Body))
    //        //{
    //            T unpackedObject = requestInfo.Body.BytesToStruct<T>();
    //            ExecuteCommand(session, unpackedObject);
    //        //}


    //    }
    //    public abstract void ExecuteCommand(CommonBinarySession session, T pack);
    //}


    //public class Test : CommonBaseCommand<DataPackage>
    //{
    //    public override void ExecuteCommand(CommonBinarySession session, DataPackage pack)
    //    {
    //        CommonAppServer server = session.AppServer as CommonAppServer;
    //        if (server != null)
    //        {
    //            server.PrintMsg(string.Format(" {0} says:{1} ", session.RemoteEndPoint, pack.Name + ":" + Encoding.UTF8.GetString(pack.Body)));
    //        }
    //        //Console.WriteLine(pack.Name + ":" + pack.Other);
    //    }
    //}


    public class Text : CommandBase<CommonBinarySession, CommonRequestInfo>
    {
        public override void ExecuteCommand(CommonBinarySession session, CommonRequestInfo pack)
        {
            CommonAppServer server = session.AppServer as CommonAppServer;
            
            if (server != null)
            {
                server.PrintMsg($"({session.RemoteEndPoint}):{Encoding.UTF8.GetString(pack.Body)}");
            }
        }
    }
    
    
    public class Voice : CommandBase<CommonBinarySession, CommonRequestInfo>
    {
        public override void ExecuteCommand(CommonBinarySession session, CommonRequestInfo pack)
        {
            CommonAppServer server = session.AppServer as CommonAppServer;

            if (server != null)
            {
                //var mAll = server.GetAllSessions();
                //if (mAll.Any())
                //{

                //    mAll.All(mClient =>
                //    {
                //        string mClientAddr = mClient.RemoteEndPoint.ToString();
                //        string mRemoteAddr = session.RemoteEndPoint.ToString();
                //        if (mClient.Connected &&!mClientAddr.Equals(mRemoteAddr))
                //        {
                //            var senddata = pack.Body.ToSendData(CommonCommands.Voice);
                //            mClient.Send(senddata);
                            
                //        }
                //        return true;
                //    });
                //}
                if (server != null)
                {
                    server.GetSessions(i => i.SessionID != session.SessionID).All(mClient =>
                    {
                        Task.Run(() =>
                        {
                            var senddata = pack.Body.ToSendData(CommonCommands.Voice);
                            mClient.Send(senddata);
                        });
                        return true;
                        ;
                    });
                }
                
                //publicVar.wBufferProvider.AddSamples(pack.Body,0,pack.Body.Length);
                //publicVar.mOutStream.Play();

            }
            //Console.WriteLine(pack.Name + ":" + pack.Other);
        }
    }

    //public class File : CommandBase<CommonBinarySession, CommonRequestInfo>
    //{
    //    public override void ExecuteCommand(CommonBinarySession session, CommonRequestInfo pack)
    //    {
    //        CommonAppServer server = session.AppServer as CommonAppServer;
    //        //FileDataBuffer
    //        if (!pack.IsValid)
    //        {
    //            server.PrintMsg(string.Format(" 收到 {0} 传来的文件包损坏.(序号:{1}) ", session.RemoteEndPoint,pack.Seq));
    //            return;
    //        }
    //        FileRequestInfo fileInfo= pack.Body.BytesToObject<FileRequestInfo>();
    //        session.FileDataBuffer.AddOrUpdate(fileInfo.Id, new List<FileRequestInfo> { fileInfo },
    //                (key, oldValue) =>
    //                {
    //                    oldValue.Add(fileInfo);
    //                    return oldValue;
    //                });
    //        if (server != null)
    //        {
    //            server.PrintMsg(string.Format(" 收到 {0} 传来的文件包. ", session.RemoteEndPoint));
    //        }

    //        if (fileInfo.IsEnd)
    //        {
    //            ValidDataArray(session, fileInfo.Id);
    //        }

    //        //Console.WriteLine(pack.Name + ":" + pack.Other);
    //    }

    //    /// <summary>
    //    /// 收到结束包之后，验证数据是否完整.
    //    /// </summary>
    //    bool ValidDataArray(CommonBinarySession session,string fileId)
    //    {
    //        List<FileRequestInfo> list;
    //        if (!session.FileDataBuffer.TryGetValue(fileId, out list))
    //        {
    //            return false;
    //        }
    //        var endItem = list.FirstOrDefault(i => i.IsEnd);
    //        if (endItem.PackageCount != list.Count)//如果包不一致，则出现了丢包的情况.
    //        {
    //            throw new Exception("丢包咯"); //todo: 处理丢包问题
    //        }
    //        var segment = new ArraySegment<byte>();

    //        //segment
    //        byte[] dataBytes = new byte[endItem.FileSize];
    //        list.OrderBy(i => i.Seq).Select(i => dataBytes.Concat(i.PackData));





    //    }


    //    /// <summary>
    //    /// 发送请求，要求补发丢失包.
    //    /// </summary>
    //    void ReSendData()
    //    {
            
    //    }
    //    /// <summary>
    //    /// 包验证通过后，重组文件.
    //    /// </summary>
    //    void UnPackageFileData(byte[] fileData,FileRequestInfo item)
    //    {
    //        System.IO.FileStream fileStreamReader;
    //        //追加流
    //        System.IO.FileStream fileStreamWriter = new System.IO.FileStream(pp, System.IO.FileMode.Create, System.IO.FileAccess.Write);
    //        foreach (string s in files)
    //        {
    //            fileStreamReader = new System.IO.FileStream(s, System.IO.FileMode.Open, System.IO.FileAccess.Read);
    //            byte[] bytes = new byte[fileStreamReader.Length];
    //            fileStreamReader.Read(bytes, 0, bytes.Length);
    //            fileStreamReader.Close();

    //            //byte数组追加
    //            fileStreamWriter.Write(bytes, 0, bytes.Length);

    //        }
    //        fileStreamWriter.Flush();
    //        fileStreamWriter.Close();
    //    }

    //}

}
