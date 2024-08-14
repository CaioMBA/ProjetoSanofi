import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:sanofi_app/Components/Widgets/CommonBorderRoundedButton.dart';
import 'package:sanofi_app/Domain/Settings/Extensions/StringExtensions.dart';
import 'package:sanofi_app/Domain/Settings/GlobalSchematics.dart';

class CommonAlertBox extends StatelessWidget {
  final String message;
  final String type;
  const CommonAlertBox({super.key, required this.message, required this.type});

  @override
  Widget build(BuildContext context) {
    Color color;
    Image icon;
    switch (type.toLowerCase()) {
      case 'error':
        color = GlobalSchematics().redColor;
        icon = Image.asset('Icons/error.png');
        break;
      case 'success':
        color = GlobalSchematics().greenColor;
        icon = Image.asset('Icons/success.png');
        break;
      case 'warning':
        color = GlobalSchematics().yellowColor;
        icon = Image.asset('Icons/warning.png');
        break;
      default:
        color = GlobalSchematics().primaryColor;
        icon = Image.asset('Icons/ofa_tech_emblem_ico.ico');
    }


    return AlertDialog(
      backgroundColor: Colors.transparent,
      content: Container(
        decoration: BoxDecoration(
          color: Colors.white,
          borderRadius: BorderRadius.circular(30),
          border: Border.all(color: color, width: 2),
        ),
        height: MediaQuery.of(context).size.height * 0.35,
        width: MediaQuery.of(context).size.width * 0.15,
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.center,
          mainAxisAlignment: MainAxisAlignment.start,
          children: [
            Container(
              height: MediaQuery.of(context).size.height * 0.05,
              decoration: BoxDecoration(
                color: color,
                borderRadius: const BorderRadius.only(
                  topLeft: Radius.circular(25),
                  topRight: Radius.circular(25),
                ),
              ),
              child: Center(
                child: icon,
              ),
            ),
            SizedBox(
              height: MediaQuery.of(context).size.height * 0.28,
              width: MediaQuery.of(context).size.width * 0.15,
              child:Column(
                crossAxisAlignment: CrossAxisAlignment.center,
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  Text(
                    '$type !'.toLowerCase().capitalize(),
                    style: TextStyle(
                        color: Colors.black,
                        fontSize: 25,
                        fontFamily: GoogleFonts.kadwa().fontFamily,
                        fontWeight: FontWeight.bold),
                  ),
                  Text(
                    message,
                    textAlign: TextAlign.center,
                    style: TextStyle(
                        color: Colors.black,
                        fontSize: 15,
                        fontFamily: GoogleFonts.kadwa().fontFamily,
                        fontWeight: FontWeight.bold),
                  ),
                  CommonBorderRoundedButton(
                      onTap: () => Navigator.pop(context),
                      text: 'close',
                      color: color,
                      width: 0.08,
                      height: 0.055,
                      fontSize: 15,
                      icon: Icon(Icons.close,color: GlobalSchematics().whiteColor,)
                  )
                ],
              )
            )
          ]
        ),
      ),
    );
  }
}
