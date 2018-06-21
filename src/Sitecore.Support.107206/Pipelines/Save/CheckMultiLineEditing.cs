using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.Save;
using System.Collections.Generic;

namespace Sitecore.Support.Pipelines.Save
{
  public class CheckMultiLineEditing
  {
    public void Process(SaveArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      this.VerifyMultiLineField(args);
    }

    private SaveArgs VerifyMultiLineField(SaveArgs saveArgs)
    {
      Assert.ArgumentNotNull(saveArgs.Items, "arsaveArgs.Itemsgs");
      List<SaveArgs.SaveItem> list = new List<SaveArgs.SaveItem>();
      for (int i = 0; i < saveArgs.Items.Length; i++)
      {
        bool flag = false;
        var iD = saveArgs.Items[i].ID;
        var language = saveArgs.Items[i].Language;
        var version = saveArgs.Items[i].Version;
        Item item = null;
        if (((!iD.IsNull && (language != null)) && (!string.IsNullOrEmpty(language.ToString()) && (version != null))) && !string.IsNullOrEmpty(version.ToString()))
        {
          item = Context.Database.GetItem(iD, language, version);
        }
        else if ((language != null) && !string.IsNullOrEmpty(language.ToString()))
        {
          item = Context.Database.GetItem(iD, language);
        }
        else
        {
          item = Context.Database.GetItem(iD);
        }
        int num2 = 0;
        if (((saveArgs.Items[i].Fields != null) && (item != null)) && ((item.Fields != null) && (item.Fields.Count > 0)))
        {
          for (int j = 0; j < saveArgs.Items[i].Fields.Length; j++)
          {
            Field field = item.Fields[saveArgs.Items[i].Fields[j].ID];
            if (field.TypeKey == "multi-line text")
            {
              num2++;
            }
            string str = field.Value;
            if (!string.Equals(saveArgs.Items[i].Fields[j].Value.Replace("\r\n", "\n"), str.Replace("\r\n", "\n")))
            {
              flag = true;
              break;
            }
          }
        }
        if ((num2 == 0) | flag)
        {
          list.Add(saveArgs.Items[i]);
        }
      }
      saveArgs.Items = list.ToArray();
      return saveArgs;
    }
  }
}