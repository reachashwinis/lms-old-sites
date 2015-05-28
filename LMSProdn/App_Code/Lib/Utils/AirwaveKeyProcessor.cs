using System;

/// <summary>
/// Summary description for AirwaveKeyProcessor
/// </summary>
public class AirwaveKeyProcessor
{
    private string _OrgName = string.Empty;
    private string _IPAddress = string.Empty;
    public string TheKey;

    private string _CertId = string.Empty;
    private string _serNumber = string.Empty;

    private string _Order = string.Empty;
    private string _product = string.Empty;
    private string _package = string.Empty;
    private int _aps = 0;
    private bool _rapids = false;
    private bool _visualrf = false;
    private double _expires = 0.0;
    private string _expiresOn = string.Empty;
    private string _generated = string.Empty;

    private bool isProcessed;
    public AirwaveKeyProcessor()
    {
        isProcessed = false;

    }
    private void Process()
    {
        string temp = TheKey.Replace("\n", "~");
        temp = temp.Replace("<BR>", "~");
        temp = temp.Replace("~~", "~");
        string[] arr = temp.Split('~');
        foreach (string s in arr)
        {
            if (s.ToLower().Contains("order"))
            {
                _Order = s.Replace(" ", "").Split(':')[1].Trim();
            }
            else if (s.ToLower().Contains("product"))
            {
                _product = s.Replace(" ", "").Split(':')[1].Trim();
            }
            else if (s.ToLower().Contains("package"))
            {
                _package = s.Replace(" ", "").Split(':')[1].Trim();
            }
            else if (s.ToLower().Contains("aps"))
            {
                _aps = Int32.Parse(s.Replace(" ", "").Split(':')[1].Trim());
            }
            else if (s.ToLower().Contains("rapids"))
            {
                _rapids = false;
                if (s.Replace(" ", "").Split(':')[1].Trim().ToLower() == "yes")
                    _rapids = true;
            }
            else if (s.ToLower().Contains("visualrf"))
            {
                _visualrf = false;
                if (s.Replace(" ", "").Split(':')[1].Trim().ToLower() == "yes")
                    _visualrf = true;
            }
            else if (s.ToLower().Contains("expires") && !s.ToLower().Contains("expires_on"))
            {
                _expires = Double.Parse(s.Replace(" ", "").Split(':')[1].Trim());
            }
            else if (s.ToLower().Contains("expires_on"))
            {
                _expiresOn = s.Replace(" ", "").Split(':')[1].Trim();
            }
            else if (s.ToLower().Contains("serial"))
            {
                _serNumber = s.Replace(" ", "").Split(':')[1].Trim();
            }
            else if (s.ToLower().Contains("generated"))
            {
                _generated = s.Replace(" ", "").Split(':')[1].Trim();
            }
            else if (s.ToLower().Contains("certificate_id"))
            {
                _CertId = s.Replace(" ", "").Split(':')[1].Trim();
            }
            else if (s.ToLower().Contains("organization"))
            {
                _OrgName = s.Replace(" ", "").Split(':')[1].Trim();
            }
            else if (s.ToLower().Contains("ip_address"))
            {
                _IPAddress = s.Replace(" ", "").Split(':')[1].Trim();
            }

        }
        isProcessed = true;

    }



    public string CertificateId
    {
        get
        {
            if (!isProcessed)
            {
                Process();
            }

            return _CertId;

        }
        set { }

    }
    public string SerialNumber
    {
        get
        {
            if (!isProcessed)
            {
                Process();
            }

            return _serNumber;

        }
        set { }


    }
    public string OrderId
    {
        get
        {
            if (!isProcessed)
            {
                Process();
            }

            return _Order;

        }
        set { }


    }
    public string Product
    {
        get
        {
            if (!isProcessed)
            {
                Process();
            }

            return _product;

        }
        set { }


    }
    public string Package
    {
        get
        {
            if (!isProcessed)
            {
                Process();
            }

            return _package;

        }
        set { }


    }
    public int APCount
    {
        get
        {
            if (!isProcessed)
            {
                Process();
            }

            return _aps;

        }
        set { }


    }
    public bool RAPIDs
    {
        get
        {
            if (!isProcessed)
            {
                Process();
            }

            return _rapids;

        }
        set { }


    }
    public bool VisualRF
    {
        get
        {
            if (!isProcessed)
            {
                Process();
            }

            return _visualrf;

        }
        set { }


    }
    public string Organization
    {
        get
        {
            if (!isProcessed)
            {
                Process();
            }

            return _OrgName;

        }
        set { }

    }
    public string IPAddress
    {
        get
        {
            if (!isProcessed)
            {
                Process();
            }

            return _IPAddress;

        }
        set { }

    }

}
