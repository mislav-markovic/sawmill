using System;
using System.Collections.Generic;
using System.Text;

namespace SawMill.Data.Models
{
  public class AlertGroupAlert
  {
    public int AlertGroupId { get; set; }
    public int AlertId { get; set; }
    public int Position { get; set; }
    public bool Not { get; set; }
    public virtual AlertGroup AlertGroup { get; set; }
    public virtual Alert Alert { get; set; }
  }
}
