using System;

namespace BLL.Infrastructure.Commands;
public class ActivateRFIDReaderCommand : CommandBase
{
    public Guid BillId { get; set; }
}
