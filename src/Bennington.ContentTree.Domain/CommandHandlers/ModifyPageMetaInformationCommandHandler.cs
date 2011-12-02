using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bennington.ContentTree.Domain.AggregateRoots;
using Bennington.ContentTree.Domain.Commands;
using SimpleCqrs.Commanding;

namespace Bennington.ContentTree.Domain.CommandHandlers
{
    public class ModifyPageMetaInformationCommandHandler : AggregateRootCommandHandler<ModifyPageMetaInformationCommand, Page>
    {
        public override void Handle(ModifyPageMetaInformationCommand command, Page page)
        {
            page.SetMetaTitle(command.MetaTitle);
            page.SetMetaKeyword(command.MetaKeywords);
            page.SetMetaDescription(command.MetaDescription);
        }
    }
}
