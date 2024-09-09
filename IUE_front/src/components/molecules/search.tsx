import {
  InputGroup,
  Input,
  Box,
  InputLeftElement,
  Button,
} from "@chakra-ui/react";
import { SearchIcon } from "@chakra-ui/icons";
import { getData } from "../atoms/grpc-client";
import { useState } from "react";
import { DataResponse } from "../atoms/IDataResponse";
import { v4 as uuidv4 } from "uuid";

const Search = (userId: string, searchedString: string) => {
  const [status, setStatus] = useState("");
  const [queryId, setQueryId] = useState("");

  const handleSearchButton = async () => {
    try {
      setQueryId(uuidv4());
      const response = (await getData(
        userId,
        queryId,
        searchedString
      )) as DataResponse;

      setStatus(response.status);
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <Box>
      <InputGroup>
        <InputLeftElement pointerEvents="none">
          <SearchIcon color="gray.300" />
        </InputLeftElement>
        <Input type="search" placeholder="searchBox"></Input>
      </InputGroup>
      <Button onClick={handleSearchButton}>Search</Button>
    </Box>
  );
};

export default Search;
