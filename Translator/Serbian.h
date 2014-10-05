#include <map>

struct Serbian
{
	using letter = char;
	using letters = std::basic_string < letter >;

	//const char const *id = "SR";
	const std::string name = "Srpski";

	const letter *alphabet;

	enum word_type { именица, заменица, глагол };

	enum attribute_categories { падеж, број, род };

	enum attributes { номинатив, генитив, датив, акузатив };

	//enum падеж_category { номинатив, генитив, датив, акузатив };

	std::map<attributes, attribute_categories> belongs_to_category;

	//struct word dictionary_words = { { "Ja", "ja" } };
};