grammar ParagrafIndledning;

/*
 * Parser Rules
 */

 /*root definition */
//I �kologilov, lov nr. 463 af 17. juni 2008, foretages f�lgende �ndring:
paragrafIndledning
	: dokumentPhrase ', foretages f�lgende �ndring' 'er'? ('.'|':') EOF
	;
//�kologilov, lov nr. 463 af 17. juni 2008
dokumentPhrase
	:TITLE 'jf. '? dokumentReference
	;

//title
//	:START_TITLE FREETEXT END_TITLE
//	;

//lov nr. 463 af 17. juni 2008
dokumentReference
	: //'lov nr. 463 af 17. juni 2008'
	DOCTYPE ' nr.' INT 'af' date
    ;
//number
//	:INT
//	;
//17. juni 2008
date:
	day MONTH year
	;
day
	: INT '.'
	;
year
	:INT
	;
/*
 * Lexer Rules
 */

WS
	:	' ' -> channel(HIDDEN)
	;

INT : [0-9]+;
//YEAR:INT;
//DAY: INT '.';
DOCTYPE: 'lov'|'lovbekendtg�relse';
MONTH: 'januar'|'februar'|'marts'|'april'|'maj'|'juni'|'juli'|'august'|'september'|'oktober'|'november'|'december';
///MONTH: 'januar'|'juni'|'juli';
LETTER : [a-z]|[A-Z]; 
fragment FREETEXT: .*?;

START_TITLE: 'I ';
END_TITLE: ', ';
TITLE: START_TITLE FREETEXT END_TITLE;

