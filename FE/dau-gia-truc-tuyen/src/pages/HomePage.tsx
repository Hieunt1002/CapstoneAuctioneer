import SearchBox from '../components/SearchBox';
import PropertiesList from '../components/properties-list/PropertiesList';
import NewsList from '../components/news-list/NewsList';
import Footer from '@common/footer/Footer';
import { useState } from 'react';
import { Auction } from '../types/auction.type';
import { getSearchAuction } from '@queries/AuctionAPI';

const HomePage = () => {
  const [searchResults, setSearchResults] = useState<Auction[]>([]);
  const [isSearch, setIsSearch] = useState(false);
  const handleSubmit = async (categoryId: number, value: string) => {
    try {
      const response = await getSearchAuction(categoryId, value);
      setIsSearch(true);
      setSearchResults(response);
    } catch (error) {
      console.error('Error searching auctions:', error);
    }
  };
  return (
    <>
      <div className="flex flex-col mt-16">
        <div className="w-full h-[450px] relative">
          <img src="banner.jpg" alt="banner-img" className="object-cover h-full w-full" />

          <SearchBox handleSubmit={handleSubmit}/>

          <div className="flex flex-col">
            <div></div>

            <div className="bg-white">
              <div className=" max-w-1440px mt-28 ml-auto mr-auto">
                <PropertiesList searchResults={searchResults} isSearch={isSearch} setIsSearch={setIsSearch} />
              </div>
            </div>
            <div className="bg-lightGray">
              <div className=" max-w-1440px ml-auto mr-auto pl-0 pr-0 pt-8">
                <NewsList />
              </div>
            </div>
            <div></div>
          </div>
          <Footer />
        </div>
      </div>
    </>
  );
};
export default HomePage;
