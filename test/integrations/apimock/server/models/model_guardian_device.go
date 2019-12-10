/*
 * Firefox Guardian API
 *
 * API to manage Guardian accounts, devices and servers
 *
 * API version: 0.1
 * Generated by: OpenAPI Generator (https://openapi-generator.tech)
 */

package models

type GuardianDevice struct {

	// Human friendly name of the device.
	Name string `json:"name,omitempty"`

	// The WireGuard public key for the device.
	Pubkey string `json:"pubkey,omitempty"`

	// The ipv4 peer adress for WireGuard. Note that the mask may be bigger then a single IP.
	Ipv4Address string `json:"ipv4_address,omitempty"`

	// The ipv6 peer address for WireGuard. Note that the mask may be bigger then a single IP.
	Ipv6Address string `json:"ipv6_address,omitempty"`

	// Datetime when the device first authenticated.
	CreatedAt string `json:"created_at,omitempty"`
}