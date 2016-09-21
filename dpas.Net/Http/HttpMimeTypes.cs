using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpas.Net.Http
{
    public static class FileExt
    {
        public const string Htm  = ".htm"; 
        public const string Html = ".html";
        public const string Css  = ".css";
        public const string Js   = ".js";
        public const string Jpg  = ".jpg";
        public const string Jpeg = ".jpeg";
        public const string Png  = ".png";
        public const string Gif  = ".gif";
    }

    public static class Mime
    {
        public static class Application //Внутренний формат прикладной программы
        {
            public const string Atom_xml     = "application/atom+xml";     // Atom
            public const string Edi_x12      = "application/EDI-X12";      // EDI X12 (RFC 1767)
            public const string Edifact      = "application/EDIFACT";      // EDI EDIFACT (RFC 1767)
            public const string Json         = "application/json";         // JavaScript Object Notation JSON (RFC 4627)
            public const string Javascript   = "application/javascript";   // JavaScript (RFC 4329)
            public const string Octet_stream = "application/octet-stream"; // двоичный файл без указания формата (RFC 2046)[3]
            public const string Ogg          = "application/ogg";          // Ogg (RFC 5334)
            public const string Pdf          = "application/pdf";          // Portable Document Format, PDF (RFC 3778)
            public const string Postscript   = "application/postscript";   // PostScript (RFC 2046)
            public const string Soap_xml     = "application/soap+xml";     // SOAP (RFC 3902)
            public const string Font_woff    = "application/font-woff";    // Web Open Font Format[4]
            public const string Xhtml_xml    = "application/xhtml+xml";    // XHTML (RFC 3236)
            public const string Xml_dtd      = "application/xml-dtd";      // DTD (RFC 3023)
            public const string Xop_xml      = "application/xop+xml";      // XOP
            public const string Zip          = "application/zip";          // ZIP[5]
            public const string Gzip         = "application/gzip";         // Gzip
            public const string Unknown      = "application/unknown";

            public static class X
            {
                public const string Bittorrent = "application/x-bittorrent"; // BitTorrent
                public const string Tex        = "application/x-tex";        // TeX


                public const string Www_form_urlencoded = "application/x-www-form-urlencoded"; // Form Encoded Data[18]
                public const string Dvi                 = "application/x-dvi";                 // DVI
                public const string Latex               = "application/x-latex";               // LaTeX файлы
                public const string Font_ttf            = "application/x-font-ttf";            // TrueType (не зарегистрированный MIME-тип, но наиболее часто используемый)
                public const string Shockwave_flash     = "application/x-shockwave-flash";     // Adobe Flash[19] и[20]
                public const string Stuffit             = "application/x-stuffit";             // StuffIt
                public const string Rar_compressed      = "application/x-rar-compressed";      // RAR
                public const string Tar                 = "application/x-tar";                 // Tarball
                public const string Javascript          = "application/x-javascript";

                //x-pkcs[править | править вики-текст]
                //PKCS

                //application/x-pkcs12: p12 файлы
                //application/x-pkcs12: pfx файлы
                //application/x-pkcs7-certificates: p7b файлы
                //application/x-pkcs7-certificates: spc файлы
                //application/x-pkcs7-certreqresp: p7r файлы
                //application/x-pkcs7-mime: p7c файлы
                //application/x-pkcs7-mime: p7m файлы
                //application/x-pkcs7-signature: p7s файлы
            }
        }
//audio[править | править вики-текст]
//Аудио

//audio/basic: mulaw аудио, 8 кГц, 1 канал (RFC 2046)
//audio/L24: 24bit Linear PCM аудио, 8-48 кГц, 1-N каналов (RFC 3190)
//audio/mp4: MP4
//audio/aac: AAC
//audio/mpeg: MP3 или др. MPEG (RFC 3003)
//audio/ogg: Ogg Vorbis, Speex, Flac или др. аудио (RFC 5334)
//audio/vorbis: Vorbis (RFC 5215)
//audio/x-ms-wma: Windows Media Audio[6]
//audio/x-ms-wax: Windows Media Audio перенаправление
//audio/vnd.rn-realaudio: RealAudio[7]
//audio/vnd.wave: WAV(RFC 2361)
//audio/webm: WebM
//image[править | править вики-текст]
        public static class Image //Изображение
        {
            public const string Gif     = "image/gif";     // GIF(RFC 2045 и RFC 2046)
            public const string Jpeg    = "image/jpeg";    // JPEG (RFC 2045 и RFC 2046)
            public const string Pjpeg   = "image/pjpeg";   // JPEG[8]
            public const string Png     = "image/png";     // Portable Network Graphics[9](RFC 2083)
            public const string Svg_xml = "image/svg+xml"; // SVG[10]
            public const string Tiff    = "image/tiff";    // TIFF(RFC 3302)
            public static class Vnd
            {
                public static class Microsoft
                {
                    public const string icon = "image/vnd.microsoft.icon"; // ICO[11]
                }
                public static class Wap
                {
                    public const string Wbmp = "image/vnd.wap.wbmp"; // WBMP
                }
            }
        }

        public static class Message //Сообщение
        {
            public const string Http     = "message/http";     // (RFC 2616)
            public const string Imdn_xml = "message/imdn+xml"; // IMDN (RFC 5438)
            public const string Partial  = "message/partial";  // E-mail (RFC 2045 и RFC 2046)
            public const string Rfc822   = "message/rfc822";   // E-mail; EML файлы, MIME файлы, MHT файлы, MHTML файлы (RFC 2045 и RFC 2046)
        }
//model[править | править вики-текст]
//Для 3D моделей

//model/example: (RFC 4735)
//model/iges: IGS файлы, IGES файлы (RFC 2077)
//model/mesh: MSH файлы, MESH файлы (RFC 2077), SILO файлы
//model/vrml: WRL файлы, VRML файлы (RFC 2077)
//model/x3d+binary: X3D ISO стандарт для 3D компьютерной графики, X3DB файлы
//model/x3d+vrml: X3D ISO стандарт для 3D компьютерной графики, X3DV VRML файлы
//model/x3d+xml: X3D ISO стандарт для 3D компютерной графики, X3D XML файлы
//multipart[править | править вики-текст]
//multipart/mixed: MIME E-mail (RFC 2045 и RFC 2046)
//multipart/alternative: MIME E-mail (RFC 2045 и RFC 2046)
//multipart/related: MIME E-mail (RFC 2387 и используемое MHTML (HTML mail))
//multipart/form-data: MIME Webform (RFC 2388)
//multipart/signed: (RFC 1847)
//multipart/encrypted: (RFC 1847)

        public static class Text  //Текст
        {
            public const string Cmd        = "text/cmd";        // команды
            public const string Css        = "text/css";        // Cascading Style Sheets (RFC 2318)
            public const string Csv        = "text/csv";        // CSV (RFC 4180)
            public const string Html       = "text/html";       // HTML (RFC 2854)
            public const string Javascript = "text/javascript"; // (Obsolete): JavaScript(RFC 4329)
            public const string Plain      = "text/plain";      // текстовые данные (RFC 2046 и RFC 3676)
            public const string Php        = "text/php";        // Скрипт языка PHP
            public const string Xml        = "text/xml";        // Extensible Markup Language (RFC 3023)

            public static class X
            {
                public const string Plain = "text/x-jquery-tmpl"; //jQuery
            }
        }

        public static class Video //Видео
        {
            public const string Mpeg1     = "video/mpeg";       // MPEG-1 (RFC 2045 и RFC 2046)
            public const string Mp4       = "video/mp4";        // MP4 (RFC 4337)
            public const string Ogg       = "video/ogg";        // Ogg Theora или другое видео (RFC 5334)
            public const string Quicktime = "video/quicktime";  // QuickTime[12]
            public const string Webm      = "video/webm";       // WebM
            public const string Ms_wmv    = "video/x-ms-wmv";   // Windows Media Video[6]
            public const string Flv       = "video/x-flv";      // FLV
            public const string _3gpp     = "video/3gpp";       // .3gpp .3gp [13]
            public const string _3gpp2    = "video/3gpp2";      // .3gpp2 .3g2 [13]
        }
//vnd[править | править вики-текст]
//Вендорные файлы

//application/vnd.oasis.opendocument.text: OpenDocument[14]
//application/vnd.oasis.opendocument.spreadsheet: OpenDocument[15]
//application/vnd.oasis.opendocument.presentation: OpenDocument[16]
//application/vnd.oasis.opendocument.graphics: OpenDocument[17]
//application/vnd.ms-excel: Microsoft Excel файлы
//application/vnd.openxmlformats-officedocument.spreadsheetml.sheet: Microsoft Excel 2007 файлы
//application/vnd.ms-powerpoint: Microsoft Powerpoint файлы
//application/vnd.openxmlformats-officedocument.presentationml.presentation: Microsoft Powerpoint 2007 файлы
//application/msword: Microsoft Word файлы
//application/vnd.openxmlformats-officedocument.wordprocessingml.document: Microsoft Word 2007 файлы
//application/vnd.mozilla.xul+xml: Mozilla XUL файлы
//application/vnd.google-earth.kml+xml: KML файлы (например, для Google Earth)




    }
}
