using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using WebEditor.Services;

namespace WebEditor.Hubs
{
    public class CodeHub : Hub
    {
        private ProjectService _service = new ProjectService();
        public void JoinDocument(int documentID)
        {
            Groups.Add(Context.ConnectionId, Convert.ToString(documentID));
        }

        public void OnChange(object changeData, int documentID)
        {
            Clients.Group(Convert.ToString(documentID), Context.ConnectionId).OnChange(changeData);
        }
        public void saveToDb(string code, int documentID)
        {
            var tempFile = _service.getFileById(documentID);
            tempFile.content = code;
            _service.updateFile(tempFile);
        }
    }
}