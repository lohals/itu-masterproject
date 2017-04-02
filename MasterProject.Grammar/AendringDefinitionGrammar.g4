grammar AendringDefinitionGrammar;

/*
 * Parser Rules
 */
 /*root definition */
aendringDefinition
	: phrase ('.'|':')? EOF
	;

/* possible AD phrase types */
phrase
	:parentTargetChangedExp
	|parentTargetRemovedExp
	
	
	|insertLastExp
	|insertAfterExp
	|insertAfterChainExp
	|insertBeforeExp

	|removeExp

	|globalReplaceExp
	|replaceExp
	
	

	|manualExp
	;

/* phrases */
globalReplaceExp
	:'Overalt i loven' replaceAktionExp multiQuotedTextChangeExp
	;
parentTargetChangedExp
	: 'Paragraffen affattes s�ledes'
	;
parentTargetRemovedExp
	: 'Paragraffen udg�r'
	;

insertAfterChainExp
	: 'I' elementChainExp insertAfterAktionExp (quotedTextChangeExp|(lastElementExp asnewElementExp?)) 
	;
insertAfterExp:
    'Efter' (elementExp) ('inds�ttes som nyt stykke'|'inds�ttes som nye stykker'|'inds�ttes som ny paragraf'|'inds�ttes')
	;										  
	
insertLastExp
	: 'Som' lastElementExp 'inds�ttes'
	| 'I' elementChainExp ('inds�ttes som'|', inds�ttes som') lastElementExp ('og' INT)?
	;
insertBeforeExp
	: 'F�r' elementChainExp 'inds�ttes som nyt nummer'
	|'I' elementChainExp (', inds�ttes f�r'|'inds�ttes f�r') (elementOrOpregningExp ('som nye stykker'|'som nye numre'))
	;

removeExp
	: elementChainExp removeAktionExp
	| 'I' elementChainExp removeAktionExp QUOTEDTEXT
	;

replaceExp
	: elementChainExp  (', affattes s�ledes'|'affattes s�ledes'|', oph�ves, og i stedet inds�ttes'|'oph�ves, og i stedet inds�ttes')
	| 'I' elementChainExp becomesExp replaceAktionExp quotedTextChangeExp
	| 'I' (elementChainExp|multiElementExp) replaceAktionExp multiQuotedTextChangeExp
	
	;
	//quotedTextChangeExp (', og' quotedTextChangeExp)?
manualExp
	:'I'? elementChainExp ((removeAktionExp QUOTEDTEXT)|('oph�ves'|', oph�ves')) manuelTextBitExp+
	|elementChainExp ((', '|'og') ignoreableElementChainExp)+ manuelTextBitExp+
	;
manuelTextBitExp:
    ', ' ignoreableElementChainExp removeAktionExp
	|(', og i' ignoreableElementChainExp removeAktionExp QUOTEDTEXT)
	|(', og' ignoreableElementChainExp (', 'ignoreableElementChainExp)? removeAktionExp) ', og i stedet inds�ttes'?
	|(', og i' ignoreableElementChainExp ('inds�ttes efter'|'inds�ttes efter') quotedTextChangeExp)
;


/*specialized meta element categories*/
elementOrOpregningExp
    :elementExp|opregningExp
	;
ignoreableElementChainExp
	:elementChainExp|multiElementExp
	;
multiElementExp:
    INT '. og' elementExp
	|elementChainExp ('og'|', og') elementChainExp
    ;
lastElementExp
	: elementExp|opregningExp
    ;

becomesExp
	:', der bliver ' elementExp
	;

/* meta element categories */
elementChainExp
    : (elementExp) (', ' elementExp)* (', ' opregningExp)*
	|elementExp 'og' elementExp
	;

elementExp 
	: paragrafExp|stkExp|pktExp
	;
opregningExp
    :nummerOpregningExp|litraOpregningExp
    ;

multiQuotedTextChangeExp
	:quotedTextChangeExp (', og' quotedTextChangeExp)*
	 ;
quotedTextChangeExp
	: QUOTEDTEXT 'til:' QUOTEDTEXT
	| QUOTEDTEXT ':' QUOTEDTEXT
	| QUOTEDTEXT '�ndres til:'QUOTEDTEXT
	| QUOTEDTEXT '�ndres til'QUOTEDTEXT
	;

/*Textual context expressions*/

removeAktionExp
	:', udg�r'|'udg�r'|',udg�r'|', oph�ves'|'oph�ves';
insertAfterAktionExp
	:', inds�ttes efter'|'inds�ttes efter'
	;
replaceAktionExp
	:', �ndres'|'�ndres'
	;
asnewElementExp:'som nyt nummer'|'som nyt stykke'|'som nye numre';
/*concret element expressions*/

nummerOpregningExp
	: 'nr. ' INT
	;

litraOpregningExp
	:'litra ' LETTER+;
paragrafExp 
	: '� ' INT LETTER?
	;
flereStkExp
	:
	'Stk. ' INT (('og'|'-'|',')INT)+
	;
stkExp 
	: ('Stk. '|'stk. ') INT
	;
pktExp
	: INT ('. pkt'|'. pkt.')
	;

/*
 * Lexer Rules
 */


INT : [0-9]+; 
LETTER : [a-z]|[A-Z]; 
QUOTEDTEXT : {_input.La(-1) == '�';}? FREETEXT {_input.La(1) == '�';}?;

fragment FREETEXT: .*?;
//SPACE:' ';
//ANYCHAR: .;

//WS
//	:	' ' -> channel(HIDDEN)
//	;