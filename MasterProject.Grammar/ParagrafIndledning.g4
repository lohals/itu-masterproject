grammar ParagrafIndledning;

/*
 * Parser Rules
 */

 /*root definition */
paragrafIndledning
	: dokumentPhraseType2|dokumentPhraseType1
	;


dokumentPhraseType1
	:TITLE 'jf.'? dokumentReference
	;

//I lov nr. 606 af 12. juni 2013 om offentlighed i forvaltningen foretages følgende ændring:
dokumentPhraseType2
	: 'I ' dokumentReference
	;

//lov nr. 463 af 17. juni 2008
dokumentReference
	: //'lov nr. 463 af 17. juni 2008'
	doctype ' nr.' INT 'af' date
    ;

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

doctype: LAW|LAWBEKENDTGORELSE;

INT : [0-9]+;
//YEAR:INT;
LAW: 'lov';
LAWBEKENDTGORELSE:'lov''\u00AD'?'bekendt''\u00AD'?'gørelse';
MONTH: 'januar'|'februar'|'marts'|'april'|'maj'|'juni'|'juli'|'august'|'september'|'oktober'|'november'|'december';
///MONTH: 'januar'|'juni'|'juli';
LETTER : [a-z]|[A-Z]; 

START_TITLE: 'I ';
END_TITLE: ', ';
TITLE: START_TITLE FREETEXT END_TITLE;


fragment FREETEXT: .*?;

WS
	:	' ' -> channel(HIDDEN)
	;