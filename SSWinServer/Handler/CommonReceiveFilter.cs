using System;
using SuperSocket.Facility.Protocol;
using Z.Lib.Model;

namespace SSWinServer.Handler
{
    public class CommonReceiveFilter : FixedHeaderReceiveFilter<CommonRequestInfo>
    {
        public CommonReceiveFilter()
            : base(14)
        {
        }

        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            var headerLengthData = new byte[4];
            Array.Copy(header, offset + 6, headerLengthData, 0, 4);
            return BitConverter.ToInt32(headerLengthData, 0)+2;
            //return (int)header[offset + 4] * 256 + (int)header[offset + 5];
        }

        protected override CommonRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            var info= new CommonRequestInfo(header,bodyBuffer,offset, length);
            return info.IsValid ? info : null;
            //return new CommonRequestInfo(Encoding.UTF8.GetString(header.Array, header.Offset, 4), bodyBuffer.CloneRange(offset, length));
        }

        
    }
}
