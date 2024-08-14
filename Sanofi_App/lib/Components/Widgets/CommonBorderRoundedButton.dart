import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:sanofi_app/Domain/Settings/GlobalSchematics.dart';

class CommonBorderRoundedButton extends StatelessWidget {
  final Function()? onTap;
  final Color? color;
  final double? fontSize;
  final double? width;
  final double? height;
  final String? text;
  final Widget? icon;
  const CommonBorderRoundedButton({super.key, required this.onTap, this.text, this.color, this.width, this.height, this.fontSize, this.icon});

  @override
  Widget build(BuildContext context) {
    return MouseRegion(
      cursor: SystemMouseCursors.click,
      child: GestureDetector(
          onTap: onTap,
          child: Container(
              height: MediaQuery.of(context).size.height * (height ?? 0.08),
              width: MediaQuery.of(context).size.width * (width ?? 0.3),
              margin: EdgeInsets.symmetric(horizontal: MediaQuery.of(context).size.width * 0.01),
              decoration: BoxDecoration(
                boxShadow: [
                  BoxShadow(
                    color: (color ?? GlobalSchematics().primaryColor).withOpacity(0.5),
                    spreadRadius: 3,
                    blurRadius: 5,
                  )
                ],
                  color: (color ?? GlobalSchematics().primaryColor),
                  borderRadius: BorderRadius.circular(15)),
              child: Row(
                crossAxisAlignment: CrossAxisAlignment.center,
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Text(
                    text ?? 'Próximo',
                    textAlign: TextAlign.center,
                    style: TextStyle(
                        color: Colors.white,
                        fontWeight: FontWeight.bold,
                        fontFamily: GoogleFonts.kadwa().fontFamily,
                        fontSize: (fontSize ?? 25),),
                  ),
                  icon ?? Container()
                ],
              ))),
    );
  }
}
