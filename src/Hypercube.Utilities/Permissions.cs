namespace Hypercube.Utilities;

[Flags]
public enum Permissions
{
    None    = 0,
    Read    = 1 << 0,
    Write   = 1 << 1,
    Execute = 1 << 2,

    ReadWrite        = Read  | Write,
    ReadExecute      = Read  | Execute,
    WriteExecute     = Write | Execute,
    ReadWriteExecute = Read  | Write | Execute
}