#include <map>

struct English
{
	using letter = char;
	using letters = std::basic_string < letter >;

	enum word_type { noun, verb, pronoun, adjective, adverb, article };

	enum attribute_categories { gender, plurality, person };

	enum attributes { present, past, future, continous, perfect };

	std::map<attributes, attribute_categories> belongs_to_category;

	//struct word dictionary_words = { { "Ja", "ja" } };
};