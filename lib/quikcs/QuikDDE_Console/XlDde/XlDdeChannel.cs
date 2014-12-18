// =========================================================================
//   XlDdeChannel.cs (c) 2013 Nikolay Moroshkin, http://www.moroshkin.com/
// =========================================================================

using System;
using System.Collections.Generic;

using NDde.Server;

namespace XlDde
{
  abstract class XlDdeChannel
  {
    // **********************************************************************

    public abstract string Topic { get; }
    public abstract void ProcessTable(XlTable xt);

    // **********************************************************************

    public event Action ConversationAdded;
    public event Action ConversationRemoved;

    // **********************************************************************

    List<DdeConversation> conversations = new List<DdeConversation>();

    // **********************************************************************

    public int Conversations { get { return conversations.Count; } }

    // **********************************************************************

    public void AddConversation(DdeConversation dc)
    {
      lock(conversations)
        conversations.Add(dc);

      if(ConversationAdded != null)
        ConversationAdded();
    }

    // **********************************************************************

    public void RemoveConversation(DdeConversation dc)
    {
      bool removed;

      lock(conversations)
        removed = conversations.Remove(dc);

      if(removed && ConversationRemoved != null)
        ConversationRemoved();
    }

    // **********************************************************************

    public DdeConversation[] DropConversations()
    {
      DdeConversation[] dcArray;

      lock(conversations)
      {
        dcArray = conversations.ToArray();
        conversations.Clear();
      }

      if(ConversationRemoved != null)
        for(int i = 0; i < dcArray.Length; i++)
          ConversationRemoved();

      return dcArray;
    }

    // **********************************************************************
  }
}
