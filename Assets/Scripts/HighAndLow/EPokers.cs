
public enum EPokers
{
    BackFace = -1,
    //heitao
    HeitaoA = 0x00,
    Heitao2 = 0x01,
    Heitao3 = 0x02,
    Heitao4 = 0x03,
    Heitao5 = 0x04,
    Heitao6 = 0x05,
    Heitao7 = 0x06,
    Heitao8 = 0x07,
    Heitao9 = 0x08,
    Heitao10 = 0x09,
    HeitaoJ = 0x0A,
    HeitaoQ = 0x0B,
    HeitaoK = 0x0C,
    //meihua
    MeihuaA = 0x10,
    Meihua2 = 0x11,
    Meihua3 = 0x12,
    Meihua4 = 0x13,
    Meihua5 = 0x14,
    Meihua6 = 0x15,
    Meihua7 = 0x16,
    Meihua8 = 0x17,
    Meihua9 = 0x18,
    Meihua10 = 0x19,
    MeihuaJ = 0x1A,
    MeihuaQ = 0x1B,
    MeihuaK = 0x1C,
    //fangkuai
    FangkuaiA = 0x20,
    Fangkuai2 = 0x21,
    Fangkuai3 = 0x22,
    Fangkuai4 = 0x23,
    Fangkuai5 = 0x24,
    Fangkuai6 = 0x25,
    Fangkuai7 = 0x26,
    Fangkuai8 = 0x27,
    Fangkuai9 = 0x28,
    Fangkuai10 = 0x29,
    FangkuaiJ = 0x2A,
    FangkuaiQ = 0x2B,
    FangkuaiK = 0x2C,
    //hongtao
    HongtaoA = 0x30,
    Hongtao2 = 0x31,
    Hongtao3 = 0x32,
    Hongtao4 = 0x33,
    Hongtao5 = 0x34,
    Hongtao6 = 0x35,
    Hongtao7 = 0x36,
    Hongtao8 = 0x37,
    Hongtao9 = 0x38,
    Hongtao10 = 0x39,
    HongtaoJ = 0x3A,
    HongtaoQ = 0x3B,
    HongtaoK = 0x3C
}

public class EPokersHelper
{
    public static string GetTextureNameFromPoker(EPokers poker) {
        var i = (int)poker / 16;
        var j = (int)poker % 16;
        return string.Format("{0}_{1}", i, j);
    }

    public static int GetIndexOfPoker(EPokers poker) {
        var i = (int)poker / 16;
        var j = (int)poker % 16;
        return i * 13 + j;
    }

    public static int GetPokerValue(EPokers poker) {
        return (int)poker % 16;
    }
}