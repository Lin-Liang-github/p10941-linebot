using Line.Messaging;
using Line.Messaging.Webhooks;

namespace p10941_linebot;

public class LineBotApp : WebhookApplication
{
    private readonly LineMessagingClient _messagingClient;
    private static Dictionary<string, string> _pool = new Dictionary<string, string>();

    public LineBotApp(LineMessagingClient lineMessagingClient)
    {
        _messagingClient = lineMessagingClient;
    }

    protected override async Task OnMessageAsync(MessageEvent ev)
    {
        var result = null as List<ISendMessage>;

        switch (ev.Message)
        {
            //文字訊息
            case TextEventMessage textMessage:
            {
                var channelId = ev.Source.Id;   //頻道Id
                var userId = ev.Source.UserId;  //使用者Id
                var text = ((TextEventMessage)ev.Message).Text; //使用者輸入的文字

                if (PoolHasMsg(text))
                {
                    // 從記憶體池查詢資料
                    string response = GetResponse(text);
                    result = new List<ISendMessage>
                    {
                        new TextMessage(response)
                    };
                }
                else
                {
                    if (CheckFormat(text))
                    {
                        TeachDog(text); //將資料寫入記憶體池
                    }
                }
                /*
                //回傳 hellow
                result = new List<ISendMessage>
                {
                    new TextMessage("輸贏!?")
                };
                */
            }
                break;
        }

        if (result != null)
            await _messagingClient.ReplyMessageAsync(ev.ReplyToken, result);
    }
    // <summary>
    // 確認是否已經學習過這個對話
    // </summary>
    // <param name="inputMsg"></param>
    // <returns></returns>
    private bool PoolHasMsg(string inputMsg)
    {
        return _pool.ContainsKey(inputMsg);
    }
    // <summary>
    // 用於 已經學習過的對話
    // </summary>
    // <param name="inputMsg"></param>
    // <returns></returns>
    private string GetResponse(string inputMsg)
    {
        return _pool[inputMsg];
    }

    private bool CheckFormat(string inputMsg)
    {
        bool result = false;
        try
        {
            string[] subs = inputMsg.Split(';');
            //檢查
            if (subs.Length == 3)
            {
                if (subs[0] == "嶺東猴子")
                {
                    result = true;
                }
            }
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp);
            throw;
        }
        return result;
    }

    private void TeachDog(string inputMsg)
    {
        try
        {
            string[] subs = inputMsg.Split(';');
            //檢查
            if (subs.Length == 3)
            {
                if (subs[0] == "嶺東猴子")
                {
                    _pool.Add(subs[1],subs[2]);
                }
            }
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp);
            throw;
        }
    }
}