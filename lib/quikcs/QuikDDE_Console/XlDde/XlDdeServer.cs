// ==========================================================================
//    XlDdeServer.cs (c) 2013 Nikolay Moroshkin, http://www.moroshkin.com/
// ==========================================================================

using System.Collections.Generic;
using NDde.Server;

namespace XlDde
{
  sealed class XlDdeServer : DdeServer
  {
    // **********************************************************************

    Dictionary<string, XlDdeChannel> channels;

    // **********************************************************************

    public XlDdeServer(string service)
      : base(service)
    {
      channels = new Dictionary<string, XlDdeChannel>();
    }

    // **********************************************************************

    public void AddChannel(XlDdeChannel channel)
    {
      channels.Add(channel.Topic, channel);
    }

    // **********************************************************************

    public void RmvChannel(XlDdeChannel channel)
    {
      channels.Remove(channel.Topic);

      foreach(DdeConversation c in channel.DropConversations())
        try { base.Disconnect(c); }
        catch { }
    }

    // **********************************************************************

    public int ChannelsCount { get { return channels.Count; } }

    // **********************************************************************

    public override void Disconnect(DdeConversation dc)
    {
      XlDdeChannel channel;
      if(channels.TryGetValue(dc.Topic, out channel))
        channel.RemoveConversation(dc);

      base.Disconnect(dc);
    }

    // **********************************************************************

    public override void Disconnect()
    {
      foreach(XlDdeChannel channel in channels.Values)
        channel.DropConversations();

      base.Disconnect();
    }

    // **********************************************************************

    protected override bool OnBeforeConnect(string topic)
    {
      return channels.ContainsKey(topic);
    }

    // **********************************************************************

    protected override void OnAfterConnect(DdeConversation dc)
    {
      XlDdeChannel channel = channels[dc.Topic];
      dc.Tag = channel;
      channel.AddConversation(dc);
    }

    // **********************************************************************

    protected override void OnDisconnect(DdeConversation dc)
    {
      ((XlDdeChannel)dc.Tag).RemoveConversation(dc);
    }

    // **********************************************************************

    protected override PokeResult OnPoke(DdeConversation dc,
      string item, byte[] data, int format)
    {
      //if(format != xlTableFormat)
      //  return PokeResult.NotProcessed;

      using(XlTable xt = new XlTable(data))
        ((XlDdeChannel)dc.Tag).ProcessTable(xt);

      return PokeResult.Processed;
    }

    // **********************************************************************
  }
}
