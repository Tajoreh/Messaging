namespace Producer.Event;

public class Message 
{
   //public
   //MessageType { get; set; }

    public Guid MessageId { get; protected set; }

    public DateTime PublishDateTime { get; protected set; }

    public string Body { get; set; }

    public Message()
    {
        MessageId = Guid.NewGuid();
        PublishDateTime = DateTime.UtcNow;
    }
}