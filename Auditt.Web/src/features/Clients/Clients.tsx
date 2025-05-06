import { useState } from "react";
import OffCanvas from "../../shared/components/OffCanvas/Index";
import { Direction } from "../../shared/components/OffCanvas/Models";
import ButtonDelete from "../../shared/components/Buttons/ButtonDelete";
import { Bar } from "../../shared/components/Progress/Bar";
import { MouseEvent } from "react";
import Swal from "sweetalert2";
import { ClientModel } from "./ClientModel";
import { ClientCreate } from "./ClientCreate";
import { ClientUpdate } from "./ClientUpdate";
import { useClient } from "./useClient";
import { LinkClients } from "../Dashboard/LinkClients";
import { ButtonUpdate } from "../../shared/components/Buttons/ButtonDetail";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faMagnifyingGlass } from "@fortawesome/free-solid-svg-icons";
export const Clients = () => {
    const [visible, setVisible] = useState(false);
    const [visibleUpdate, setVisibleUpdate] = useState(false);
    const { clients, queryClients, deleteClient } = useClient();
    const [client, setClient] = useState<ClientModel>();
    const [searClients, setSearClients] = useState('');

    const handleClickDetail = (clientSelected: ClientModel) => {
        if (clientSelected) {
            setClient(clientSelected);
            setVisibleUpdate(true);
        }
    }

    function handleDelete(e: MouseEvent<HTMLButtonElement>, id: number): void {
        e.preventDefault();
        Swal.fire({
            title: '¿Estás seguro de eliminar este Cliente?',
            text: 'Esta acción no se puede deshacer',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'Confirmar',
            cancelButtonText: 'Cancelar',
            preConfirm: async () => {
                await deleteClient.mutateAsync(id);
            }
        })
    }

    if (queryClients.isLoading)
        return <Bar/>

    const normalizeText = (text: string) =>
        text.normalize("NFD").replace(/[\u0300-\u036f]/g, "").toLowerCase();

    const search = normalizeText(searClients.trim());

    const filteredClient = clients?.filter(client => {
        const fields = `${client.name} ${client.abbreviation} ${client.nit} ${client.city}`;
        const words = normalizeText(fields).split(/\s+/);
        return words.some(word => word.startsWith(search));
    });

    return (
        <div className="p-6 w-full">
            <div>
                <div className="flex space-x-8 text-lg font-medium mb-6 mr-2">
                    <LinkClients />
                </div>
                <div className="flex">
                    <h2 className="text-2xl font-semibold mb-3 mr-2">Clientes o Instituciones</h2>
                    <div className="relative mb-3 mr-2"  >
                        <div className=" inline-flex">
                            <input type="text"
                                value={searClients}
                                onChange={(e) => setSearClients(e.target.value)}
                                placeholder="Buscar Cliente"
                                className="border rounded px-3 py-1 transition duration-200 border-gray-300 hover:border-indigo-500 
                                 hover:bg-gray-50 focus:outline-none focus:ring-2 text-center focus:ring-indigo-400"/>
                            <FontAwesomeIcon icon={faMagnifyingGlass} className="fas fa-search absolute left-3 top-3 text-gray-400" />
                        </div>
                    </div>
                    <button onClick={() => setVisible(true)} className=" cursor-pointer bg-[#392f5a] cursor-por hover:bg-indigo-900 text-white px-5 rounded-lg font-semibold mb-3 mr-2">
                        Crear Cliente
                    </button>
                </div>
                <div>
                    <div className="grid grid-cols-5">
                        <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center">RAZON SOCIAL</div>
                        <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center">ABREVIATURA</div>
                        <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center">NIT</div>
                        <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center">CIUDAD</div>
                        <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center">OPCIONES</div>
                    </div>
                    <div className=" bg-white px-2 py-2 border border-gray-200">
                        {filteredClient?.map((client) => (
                            <div className="grid grid-cols-5 hover:bg-[#F4EDEE] transition-colors">
                                <div className="text-sm px-2 py-2 border border-gray-300 text-center">{client.name}</div>
                                <div className=" text-sm px-2 py-2 border border-gray-300 text-center">{client.abbreviation}</div>
                                <div className=" text-sm px-2 py-2 border border-gray-300 text-center">{client.nit}</div>
                                <div className=" text-sm px-2 py-2 border border-gray-300 text-center">{client.city}</div>
                                <div className="flex justify-center text-sm px-2  border border-gray-300 py-1">
                                    <div onClick={() => handleClickDetail(client)}>
                                        <ButtonUpdate />
                                    </div>
                                    <ButtonDelete id={client.id ?? 0} onDelete={handleDelete} />
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
                <OffCanvas titlePrincipal='Crear Cliente' visible={visible} xClose={() => setVisible(false)} position={Direction.Right}  >
                    <ClientCreate />
                </OffCanvas>
                {
                    client &&
                    <OffCanvas titlePrincipal='Actualizar Cliente' visible={visibleUpdate} xClose={() => setVisibleUpdate(false)} position={Direction.Right}  >
                        <ClientUpdate data={client} />
                    </OffCanvas>
                }
            </div>
        </div>
    );
}