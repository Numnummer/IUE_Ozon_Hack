import { Box } from "@chakra-ui/react";
import Search from "../molecules/search";
import { IGoods } from "../atoms/IGoods";
import { useState } from "react";

export default function Main() {
  const [Goods, SetGoods] = useState<IGoods[]>();

  return (
    <>
      <Box>
        <Search />
        <Box>
          {Goods?.map((item) => (
            <Box
              maxW="sm"
              borderWidth="1px"
              borderRadius="lg"
              overflow="hidden">
              <Box p="6">
                <Box
                  mt="1"
                  fontWeight="semibold"
                  as="h4"
                  lineHeight="tight"
                  noOfLines={1}>
                  {item.title}
                </Box>
                <Box>{item.description}</Box>
              </Box>
              <Box>{item.price}</Box>
            </Box>
          ))}
        </Box>
      </Box>
    </>
  );
}
