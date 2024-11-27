import React, { useEffect, useState } from 'react';
import { cityResponse, districtResponse, wardResponse } from '../../types/auth.type';
import { getCity, getDistrict, getWard } from '../../queries/AdminAPI';
import axiosInstance from '@services/axiosInstance';
import { useMessage } from '@contexts/MessageContext';
import { addUserInformation } from '../../queries/AdminAPI';
import { useNavigate } from 'react-router-dom';

const AddInfo = () => {
  const [citys, setCitys] = useState<cityResponse[]>([]);
  const [districts, setDistricts] = useState<districtResponse[]>([]);
  const [wards, setWards] = useState<wardResponse[]>([]);
  const [fullName, setFullName] = useState<string>('');
  const [phone, setPhone] = useState<string>('');
  const [city, setCity] = useState<string>('');
  const [district, setDistrict] = useState<string>('');
  const [selectedCityCode, setSelectedCityCode] = useState<string>('');
  const [selectedDistrictCode, setSelectedDistrictCode] = useState<string>('');
  const [selectedWardCode, setSelectedWardCode] = useState<string>('');
  const [address, setAddress] = useState<string>('');
  const [selectedImage, setSelectedImage] = useState<any>(null);
  const [selectedFrontCCCD, setSelectedFrontCCCD] = useState<any>(null);
  const [selectedBacksideCCCD, setSelectedBacksideCCCD] = useState<any>(null);
  const [signature, setSignature] = useState<any>(null);
  const [isTouched, setIsTouched] = useState(false);
  const [gender, setGender] = useState<boolean>();
  const [birthDate, setBirthDate] = useState<string>('');
  const [placeOfResidence, setPlaceOfResidence] = useState<string>('');
  const [placeOfIssue, setPlaceOfIssue] = useState<string>('');
  const [dateOfIssue, setDateOfIssue] = useState<string>('');
  const navigate = useNavigate();

  const { setErrorMessage, setSuccessMessage } = useMessage();

  useEffect(() => {
    const fetchCity = async () => {
      try {
        const cityData = await getCity();
        setCitys(cityData);
      } catch (error) {
        setErrorMessage('Error fetching city');
      }
    };
    fetchCity();
  }, []);

  useEffect(() => {
    const fetchDistricts = async () => {
      if (selectedCityCode) {
        try {
          const districtData = await getDistrict();
          setDistricts(districtData);
        } catch (error) {
          setErrorMessage('Error fetching districts');
        }
      }
    };
    fetchDistricts();
  }, [selectedCityCode]);

  useEffect(() => {
    const fetchWard = async () => {
      if (selectedDistrictCode) {
        try {
          const wardData = await getWard();
          setWards(wardData);
        } catch (error) {
          setErrorMessage('Error fetching wards');
        }
      }
    };

    fetchWard();
  }, [selectedDistrictCode]);

  const filteredDistricts = districts.filter(
    (district) => district.province_code.toString() === selectedCityCode
  );
  const filteredWards = wards.filter(
    (ward) => ward.district_code.toString() === selectedDistrictCode
  );

  const handleImageChange = (e: any) => {
    setSelectedImage(e.target.files[0]);
  };

  const handleFrontCCCD = (e: any) => {
    setSelectedFrontCCCD(e.target.files[0]);
  };

  const handleBacksideCCCD = (e: any) => {
    setSelectedBacksideCCCD(e.target.files[0]);
  };

  const handleSignature = (e: any) => {
    setSignature(e.target.files[0]);
  };

  const handleRemoveFrontImage = () => {
    setSelectedFrontCCCD(null);
  };

  const handleRemoveBackImage = () => {
    setSelectedBacksideCCCD(null);
  };
  const handleRemoveSignature = () => {
    setSignature(null);
  };

  const handleRemoveImage = () => {
    setSelectedImage(null);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsTouched(true);

    const formData = new FormData();
    formData.append('fullName', fullName);
    formData.append('gender', JSON.stringify(gender));

    formData.append('birthdate', birthDate);
    formData.append('phone', phone);
    formData.append('city', city);
    formData.append('district', district);
    formData.append('ward', selectedWardCode);
    formData.append('address', address);
    formData.append('placeOfResidence', placeOfResidence);
    formData.append('placeOfIssue', placeOfIssue);

    const formattedDateOfIssue = new Date(dateOfIssue).toISOString();
    formData.append('dateOfIssue', dateOfIssue);

    if (selectedImage) formData.append('avatar', selectedImage);
    if (selectedFrontCCCD) formData.append('frontCCCD', selectedFrontCCCD);
    if (selectedBacksideCCCD) formData.append('backsideCCCD', selectedBacksideCCCD);
    if (signature) formData.append('signature', signature);

    try {
      const response = await addUserInformation(formData);
      if (response.isSucceed) {
        navigate('/profile');
        setSuccessMessage('Successfully updated user information!');
      } else {
        setErrorMessage(response.message);
      }
    } catch (error) {
      setErrorMessage('Erro add infomation');
    }
  };

  return (
    <>
      <div className="w-full mt-16 flex items-center justify-center pt-10">
        <h5 className="font-bold text-center text-2xl pl-9">ADD INFORMATION</h5>
      </div>
      <form onSubmit={handleSubmit}>
        <div className="flex justify-center pt-10">
          <div className="w-1/3 p-4">
            <div className="w-full max-w-lg">
              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full  px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    họ và tên
                  </label>
                  <input
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border ${!fullName && isTouched ? 'border-red-500' : 'border-gray-200'} rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    id="grid-last-name"
                    type="text"
                    placeholder="Enter full name"
                    value={fullName}
                    onChange={(e) => setFullName(e.target.value)}
                    required
                    onFocus={() => setIsTouched(true)}
                  />
                  {!fullName && isTouched && (
                    <p className="text-red-500 text-xs italic">Please enter your full name.</p>
                  )}
                </div>
              </div>
              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-gender"
                  >
                    Giới tính
                  </label>
                  <select
                    className="appearance-none block w-full bg-gray-200 text-gray-700 border border-gray-200 rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500"
                    id="grid-gender"
                    value={gender !== null ? Number(gender) : ''}
                    onChange={(e) => setGender(e.target.value === '1')}
                    required
                  >
                    <option value="">Chon gioi tinh</option>
                    <option value="1">Nam</option>
                    <option value="0">Nu</option>
                  </select>
                  {!gender && isTouched && (
                    <p className="text-red-500 text-xs italic">Vui lòng chọn giới tính .</p>
                  )}
                </div>
              </div>
              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full  px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    Ngày sinh
                  </label>
                  <input
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border ${!birthDate && isTouched ? 'border-red-500' : 'border-gray-200'} rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    id="grid-last-name"
                    type="date"
                    placeholder="Chọn ngày cấp"
                    value={birthDate}
                    onChange={(e) => setBirthDate(e.target.value)}
                    onFocus={() => setIsTouched(true)}
                    required
                  />
                  {!birthDate && isTouched && (
                    <p className="text-red-500 text-xs italic">Vio lòng chọn giới tính .</p>
                  )}
                </div>
              </div>
              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full md:w-1/3  px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    City
                  </label>
                  <select
                    id="grid-city"
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border ${!city && isTouched ? 'border-red-500' : 'border-gray-200'} rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    value={selectedCityCode}
                    onChange={(e) => {
                      const selectedCode = e.target.value;
                      const selectedCity = citys.find(
                        (cityItem) => cityItem.code.toString() === selectedCode
                      );
                      setSelectedCityCode(selectedCity?.code.toString() || '');
                      setCity(selectedCity?.code.toString() || '');
                    }}
                    onFocus={() => setIsTouched(true)}
                  >
                    <option value="">Select a city</option>
                    {citys.map((cityItem) => (
                      <option key={cityItem.code} value={cityItem.code.toString()}>
                        {cityItem.name}
                      </option>
                    ))}
                  </select>
                  {!selectedCityCode && isTouched && (
                    <p className="text-red-500 text-xs italic">Please select a city.</p>
                  )}
                </div>
                <div className="w-full md:w-1/3  px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    Districts
                  </label>
                  <select
                    id="grid-city"
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border ${!selectedCityCode && isTouched ? 'border-red-500' : 'border-gray-200'} rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    value={selectedDistrictCode}
                    onChange={(e) => {
                      const selectDis = districts.find(
                        (districtResponse) => districtResponse.code.toString() === e.target.value
                      );
                      setDistrict(selectDis?.code.toString() || '');
                      setSelectedDistrictCode(selectDis?.code.toString() || '');
                    }}
                    onFocus={() => setIsTouched(true)}
                  >
                    <option value="">Select a districts</option>
                    {filteredDistricts.length > 0 ? (
                      filteredDistricts.map((districtItem) => (
                        <option key={districtItem.code} value={districtItem.code}>
                          {districtItem.name}
                        </option>
                      ))
                    ) : (
                      <option value="">No districts available</option>
                    )}
                  </select>
                  {!selectedCityCode && isTouched && (
                    <p className="text-red-500 text-xs italic">Please select a district.</p>
                  )}
                </div>
                <div className="w-full md:w-1/3  px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    Ward
                  </label>
                  <select
                    id="grid-city"
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border ${!selectedCityCode && isTouched ? 'border-red-500' : 'border-gray-200'} rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    value={selectedWardCode}
                    onChange={(e) => setSelectedWardCode(e.target.value)}
                    onFocus={() => setIsTouched(true)}
                  >
                    <option value="">Select a ward</option>
                    {filteredWards.length > 0 ? (
                      filteredWards.map((wardItem) => (
                        <option key={wardItem.code} value={wardItem.name}>
                          {wardItem.name}
                        </option>
                      ))
                    ) : (
                      <option value="">No wards available</option>
                    )}
                  </select>
                  {!selectedCityCode && isTouched && (
                    <p className="text-red-500 text-xs italic">Please select a ward.</p>
                  )}
                </div>
              </div>

              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full  px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    địa chỉ nhà
                  </label>
                  <input
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border ${!address && isTouched ? 'border-red-500' : 'border-gray-200'} rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    id="grid-last-name"
                    type="text"
                    placeholder="Enter full name"
                    value={address}
                    onChange={(e) => setAddress(e.target.value)}
                    required
                    onFocus={() => setIsTouched(true)}
                  />
                  {!address && isTouched && (
                    <p className="text-red-500 text-xs italic">Please enter your full name.</p>
                  )}
                </div>
              </div>

              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    số điện thoại
                  </label>
                  <input
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border ${!/^[0-9]{10}$/.test(phone) && isTouched ? 'border-red-500' : 'border-gray-200'} rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    id="grid-last-name"
                    type="text"
                    placeholder="Enter phone"
                    value={phone}
                    onChange={(e) => setPhone(e.target.value)}
                    onFocus={() => setIsTouched(true)}
                    required
                  />
                  {!/^[0-9]{10}$/.test(phone) && isTouched && (
                    <p className="text-red-500 text-xs italic">
                      Please enter a valid phone number.
                    </p>
                  )}
                </div>
              </div>
              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full  px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    hộ khẩu thường trú
                  </label>
                  <input
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border ${!placeOfResidence && isTouched ? 'border-red-500' : 'border-gray-200'} rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    id="grid-last-name"
                    type="text"
                    placeholder="Nhập hộ khẩu thường trú"
                    value={placeOfResidence}
                    onChange={(e) => setPlaceOfResidence(e.target.value)}
                    required
                    onFocus={() => setIsTouched(true)}
                  />
                  {!placeOfResidence && isTouched && (
                    <p className="text-red-500 text-xs italic">Please enter your full name.</p>
                  )}
                </div>
              </div>
            </div>
          </div>
          <div className="w-1/3 p-4">
            <div className="w-full max-w-lg">
              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full  px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    Ảnh CMND/CCCD
                  </label>
                </div>
              </div>

              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full md:w-1/2 px-3 mb-6 md:mb-0">
                  <div className="flex items-center justify-center w-full">
                    {selectedFrontCCCD ? (
                      <div className="relative w-full h-32 border-2 border-gray-300 border-dashed rounded-lg cursor-pointer">
                        <img
                          src={URL.createObjectURL(selectedFrontCCCD)}
                          alt="Front CCCD"
                          className="w-full h-full object-cover rounded-lg"
                        />
                        <button
                          className="absolute top-0 right-0 p-1 bg-red-500 text-white rounded-full w-6 h-6 flex items-center justify-center"
                          onClick={handleRemoveFrontImage}
                        >
                          X
                        </button>
                      </div>
                    ) : (
                      <label
                        htmlFor="dropzone-file"
                        className="flex flex-col items-center justify-center w-full h-32 border-2 border-gray-300 border-dashed rounded-lg cursor-pointer bg-gray-50 dark:hover:bg-gray-800 dark:bg-gray-700 hover:bg-gray-100 dark:border-gray-600 dark:hover:border-gray-500 dark:hover:bg-gray-600"
                      >
                        <div className="flex flex-col items-center justify-center pt-5 pb-6">
                          <svg
                            className="w-8 h-8 mb-4 text-gray-500 dark:text-gray-400"
                            aria-hidden="true"
                            xmlns="http://www.w3.org/2000/svg"
                            fill="none"
                            viewBox="0 0 20 16"
                          >
                            <path
                              stroke="currentColor"
                              strokeLinecap="round"
                              strokeLinejoin="round"
                              strokeWidth="2"
                              d="M13 13h3a3 3 0 0 0 0-6h-.025A5.56 5.56 0 0 0 16 6.5 5.5 5.5 0 0 0 5.207 5.021C5.137 5.017 5.071 5 5 5a4 4 0 0 0 0 8h2.167M10 15V6m0 0L8 8m2-2 2 2"
                            />
                          </svg>
                          <p className="mb-2 text-sm text-gray-500 dark:text-gray-400">
                            <span className="font-semibold">Click to upload</span> or drag and drop
                          </p>
                        </div>
                        <input
                          id="dropzone-file"
                          type="file"
                          accept="image/*"
                          className="hidden"
                          onChange={handleFrontCCCD}
                        />
                      </label>
                    )}
                  </div>
                </div>
                <div className="w-full md:w-1/2 px-3 mb-6 md:mb-0">
                  <div className="flex items-center justify-center w-full">
                    {selectedBacksideCCCD ? (
                      <div className="relative w-full h-32 border-2 border-gray-300 border-dashed rounded-lg cursor-pointer">
                        <img
                          src={URL.createObjectURL(selectedBacksideCCCD)}
                          alt="Back CCCD"
                          className="w-full h-full object-cover rounded-lg"
                        />
                        <button
                          className="absolute top-0 right-0 p-1 bg-red-500 text-white rounded-full w-6 h-6 flex items-center justify-center"
                          onClick={handleRemoveBackImage}
                        >
                          X
                        </button>
                      </div>
                    ) : (
                      <label
                        htmlFor="dropzone-file"
                        className="flex flex-col items-center justify-center w-full h-32 border-2 border-gray-300 border-dashed rounded-lg cursor-pointer bg-gray-50 dark:hover:bg-gray-800 dark:bg-gray-700 hover:bg-gray-100 dark:border-gray-600 dark:hover:border-gray-500 dark:hover:bg-gray-600"
                      >
                        <div className="flex flex-col items-center justify-center pt-5 pb-6">
                          <svg
                            className="w-8 h-8 mb-4 text-gray-500 dark:text-gray-400"
                            aria-hidden="true"
                            xmlns="http://www.w3.org/2000/svg"
                            fill="none"
                            viewBox="0 0 20 16"
                          >
                            <path
                              stroke="currentColor"
                              strokeLinecap="round"
                              strokeLinejoin="round"
                              strokeWidth="2"
                              d="M13 13h3a3 3 0 0 0 0-6h-.025A5.56 5.56 0 0 0 16 6.5 5.5 5.5 0 0 0 5.207 5.021C5.137 5.017 5.071 5 5 5a4 4 0 0 0 0 8h2.167M10 15V6m0 0L8 8m2-2 2 2"
                            />
                          </svg>
                          <p className="mb-2 text-sm text-gray-500 dark:text-gray-400">
                            <span className="font-semibold">Click to upload</span> or drag and drop
                          </p>
                        </div>
                        <input
                          id="dropzone-file"
                          type="file"
                          className="hidden"
                          accept="image/*"
                          onChange={handleBacksideCCCD}
                        />
                      </label>
                    )}
                  </div>
                </div>
              </div>
              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full  px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    Nơi cấp:
                  </label>
                  <input
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border ${!placeOfIssue && isTouched ? 'border-red-500' : 'border-gray-200'} rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    id="grid-last-name"
                    type="text"
                    placeholder="Enter full name"
                    value={placeOfIssue}
                    onChange={(e) => setPlaceOfIssue(e.target.value)}
                    required
                    onFocus={() => setIsTouched(true)}
                  />
                  {!placeOfIssue && isTouched && (
                    <p className="text-red-500 text-xs italic">Vui lòng nhập nơi cấp</p>
                  )}
                </div>
              </div>
              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full  px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    Ngày Cấp
                  </label>
                  <input
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border ${!dateOfIssue && isTouched ? 'border-red-500' : 'border-gray-200'} rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    id="grid-last-name"
                    type="date"
                    placeholder="Chọn ngày cấp"
                    value={dateOfIssue}
                    onChange={(e) => setDateOfIssue(e.target.value)}
                    onFocus={() => setIsTouched(true)}
                    required
                  />
                  {!dateOfIssue && isTouched && (
                    <p className="text-red-500 text-xs italic">Vui lòng nhập ngày cung cấp</p>
                  )}
                </div>
              </div>
              <div className="flex">
                <div className="flex flex-wrap -mx-3 mb-6">
                  <div className="w-full  px-3 mb-6 md:mb-0">
                    <label
                      className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                      htmlFor="grid-first-name"
                    >
                      Avatar
                    </label>
                  </div>
                </div>
                <div className='mt-11'>
                  {!selectedImage ? (
                    <div className="flex items-center justify-center w-44 h-44 border-2 border-gray-300 border-dashed rounded-full cursor-pointer bg-gray-50 dark:hover:bg-gray-800 dark:bg-gray-700 hover:bg-gray-100 dark:border-gray-600 dark:hover:border-gray-500 dark:hover:bg-gray-600">
                      <input
                        type="file"
                        accept="image/*"
                        onChange={handleImageChange}
                        className="hidden"
                        id="upload-avatar"
                      />
                      <label
                        htmlFor="upload-avatar"
                        className="flex flex-col items-center justify-center w-full h-full cursor-pointer"
                      >
                        <svg
                          className="w-8 h-8 mb-4 text-gray-500 dark:text-gray-400"
                          aria-hidden="true"
                          xmlns="http://www.w3.org/2000/svg"
                          fill="none"
                          viewBox="0 0 20 16"
                        >
                          <path
                            stroke="currentColor"
                            strokeLinecap="round"
                            strokeLinejoin="round"
                            strokeWidth="2"
                            d="M13 13h3a3 3 0 0 0 0-6h-.025A5.56 5.56 0 0 0 16 6.5 5.5 5.5 0 0 0 5.207 5.021C5.137 5.017 5.071 5 5 5a4 4 0 0 0 0 8h2.167M10 15V6m0 0L8 8m2-2 2 2"
                          />
                        </svg>
                        <p className="mb-2 text-sm text-gray-500 dark:text-gray-400">
                          <span className="font-semibold">Upload</span> avatar
                        </p>
                      </label>
                    </div>
                  ) : (
                    <div className="relative mt-4">
                      <img
                        src={URL.createObjectURL(selectedImage)}
                        className="rounded-full w-64 h-64 object-cover border-2 border-gray-300 border-dashed h-52"
                        alt="Selected"
                      />
                      <button
                        className="absolute top-1 right-1 p-1 bg-red-500 text-white rounded-full w-8 h-8 flex items-center justify-center"
                        onClick={handleRemoveImage}
                      >
                        <svg
                          className="w-4 h-4"
                          fill="none"
                          stroke="currentColor"
                          viewBox="0 0 24 24"
                          xmlns="http://www.w3.org/2000/svg"
                        >
                          <path
                            strokeLinecap="round"
                            strokeLinejoin="round"
                            strokeWidth="2"
                            d="M6 18L18 6M6 6l12 12"
                          ></path>
                        </svg>
                      </button>
                    </div>
                  )}
                </div>
                <div className="flex flex-wrap -mx-3 mb-6">
                  <div className="w-full  px-3 mb-6 md:mb-0">
                    <label
                      className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                      htmlFor="grid-first-name"
                    >
                      signature
                    </label>
                  </div>
                </div>
                <div className='mt-11'>
                  {signature ? (
                    <div className="relative w-full h-32 border-2 border-gray-300 border-dashed rounded-lg cursor-pointer">
                      <img
                        src={URL.createObjectURL(signature)}
                        alt="Back CCCD"
                        className="w-full h-full object-cover rounded-lg"
                      />
                      <button
                        className="absolute top-0 right-0 p-1 bg-red-500 text-white rounded-full w-6 h-6 flex items-center justify-center"
                        onClick={handleRemoveSignature}
                      >
                        X
                      </button>
                    </div>
                  ) : (
                    <label
                      htmlFor="dropzone-file"
                      className="flex flex-col items-center justify-center w-full h-32 border-2 border-gray-300 border-dashed rounded-lg cursor-pointer bg-gray-50 dark:hover:bg-gray-800 dark:bg-gray-700 hover:bg-gray-100 dark:border-gray-600 dark:hover:border-gray-500 dark:hover:bg-gray-600"
                    >
                      <div className="flex flex-col items-center justify-center pt-5 pb-6">
                        <svg
                          className="w-8 h-8 mb-4 text-gray-500 dark:text-gray-400"
                          aria-hidden="true"
                          xmlns="http://www.w3.org/2000/svg"
                          fill="none"
                          viewBox="0 0 20 16"
                        >
                          <path
                            stroke="currentColor"
                            strokeLinecap="round"
                            strokeLinejoin="round"
                            strokeWidth="2"
                            d="M13 13h3a3 3 0 0 0 0-6h-.025A5.56 5.56 0 0 0 16 6.5 5.5 5.5 0 0 0 5.207 5.021C5.137 5.017 5.071 5 5 5a4 4 0 0 0 0 8h2.167M10 15V6m0 0L8 8m2-2 2 2"
                          />
                        </svg>
                        <p className="mb-2 text-sm text-gray-500 dark:text-gray-400">
                          <span className="font-semibold">Click to upload</span> or drag and drop
                        </p>
                      </div>
                      <input
                        id="dropzone-file"
                        type="file"
                        className="hidden"
                        accept="image/*"
                        onChange={handleSignature}
                      />
                    </label>
                  )}
                </div>
              </div>
            </div>
          </div>
        </div>
        <div className="flex justify-center pb-20 pt-10">
          <button
            type="submit"
            className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
          >
            Thêm thông tin
          </button>
        </div>
      </form>
    </>
  );
};

export default AddInfo;