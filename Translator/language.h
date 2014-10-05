#include <string>

namespace translator
{
	template <class SourceLanguage, class DestinationLanguage>
	class translator
	{
		const SourceLanguage &source;
		const DestinationLanguage &destination;

	public:
		translator(const SourceLanguage& source, const DestinationLanguage& destination)
			: source(source), destination(destination)
		{ }

		typename DestinationLanguage::letters translate(const typename SourceLanguage::letters text)
		{
			std::cout << "Trying to translate : " << text << std::endl;
			return DestinationLanguage::letters();
		}

	private:
		typename DestinationLanguage::letters translate(const std::vector<const typename SourceLanguage::letters> text)
		{

		}
	};
}