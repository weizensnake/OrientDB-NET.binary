﻿using System.Linq;
using Orient.Client.Protocol.Serializers;

namespace Orient.Client.Protocol.Operations
{
    internal class DbExist : IOperation
    {
        internal string DatabaseName { get; set; }

        public Request Request(int sessionID)
        {
            Request request = new Request();

            // standard request fields
            request.DataItems.Add(new RequestDataItem() { Type = "byte", Data = BinarySerializer.ToArray((byte)OperationType.DB_EXIST) });
            request.DataItems.Add(new RequestDataItem() { Type = "int", Data = BinarySerializer.ToArray(sessionID) });
            // operation specific fields
            request.DataItems.Add(new RequestDataItem() { Type = "string", Data = BinarySerializer.ToArray(DatabaseName) });

            return request;
        }

        public ODocument Response(Response response)
        {
            ODocument document = new ODocument();

            if (response == null)
            {
                return document;
            }

            var reader = response.Reader;

            // operation specific fields
            byte existByte = reader.ReadByte();

            if (existByte == 0)
            {
                document.SetField("Exists", false);
            }
            else
            {
                document.SetField("Exists", true);
            }

            return document;
        }
    }
}
