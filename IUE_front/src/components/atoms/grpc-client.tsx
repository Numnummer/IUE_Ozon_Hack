import { DataResponse } from "./IDataResponse";
import * as grpc from "@grpc/grpc-js";
import * as protoLoader from "@grpc/proto-loader";
import { userCommunicate } from "../../../../my";
import path from "path";

const PORT = 8082;
const PROTO_PATH = path.resolve(__dirname, "../../../../my.proto");
// const host = "localhost:50051";
const packageDefinition = protoLoader.loadSync(PROTO_PATH, {
  keepCase: true,
  longs: String,
  enums: String,
  defaults: true,
  oneofs: true,
});

const protoDescriptor = grpc.loadPackageDefinition(
  packageDefinition
) as unknown as userCommunicate;
//const routeguide = protoDescriptor.routeguide;

const RpcServer = new protoDescriptor.ApiGateway.Proto.RpcServer();
const client = new RpcServer(
  `127.0.0.1:${PORT}`,
  grpc.credentials.createInsecure()
);

async function getData(
  userId: string,
  queryId: string,
  searchedString: string
) {
  return new Promise((resolve, reject) => {
    client.GetData(
      { userId, queryId, searchedString },
      (err: any, response: DataResponse) => {
        if (err) {
          reject(err);
        } else {
          resolve(response as DataResponse);
        }
      }
    );
  });
}

export { getData };
